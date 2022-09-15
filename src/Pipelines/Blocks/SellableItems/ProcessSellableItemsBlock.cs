// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessSellableItemsBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Models;
using Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments;
using Ajsuth.Sample.ImageExport.Engine.Service;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines.Blocks
{
    /// <summary>Defines the asynchronous executing ProcessSellableItems pipeline block</summary>
    /// <seealso cref="AsyncPipelineBlock{TInput, TOutput, TContext}" />
    [PipelineDisplayName(AzureStorageConstants.Pipelines.Blocks.ProcessSellableItems)]
    public class ProcessSellableItemsBlock : AsyncPipelineBlock<ExportImagesArgument, ExportImagesArgument, CommercePipelineExecutionContext>
    {
        /// <summary>Gets or sets the commander.</summary>
        /// <value>The commander.</value>
        protected CommerceCommander Commander { get; set; }

        /// <summary>Initializes a new instance of the <see cref="ProcessSellableItemsBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public ProcessSellableItemsBlock(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        /// <summary>Executes the pipeline block's code logic.</summary>
        /// <param name="arg">The pipeline argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="ExportImagesArgument"/>.</returns>
        public override async Task<ExportImagesArgument> RunAsync(ExportImagesArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument can not be null");

            long itemsProcessed = 0;

            var listName = AzureStorageConstants.Lists.SellableItems;

            var items = await GetListIds<SellableItem>(context, listName, int.MaxValue).ConfigureAwait(false);
            var listCount = items.List.TotalItemCount;

            context.Logger.LogInformation($"{Name}-Reviewing List:{listName}|Count:{listCount}|Environment:{context.CommerceContext.Environment.Name}");

            if (listCount == 0)
            {
                return arg;
            }

            itemsProcessed += listCount;

            foreach (var entityId in items.EntityReferences.Select(e => e.EntityId))
            {
                var error = false;

                var newContext = new CommercePipelineExecutionContextOptions(new CommerceContext(context.CommerceContext.Logger, context.CommerceContext.TelemetryClient)
                {
                    Environment = context.CommerceContext.Environment,
                    Headers = context.CommerceContext.Headers,
                },
                onError: x => error = true,
                onAbort: x =>
                {
                    if (!x.Contains("Ok|", StringComparison.OrdinalIgnoreCase))
                    {
                        error = true;
                    }
                });

                newContext.CommerceContext.AddObject(context.CommerceContext.GetObject<ExportResult>());
                newContext.CommerceContext.AddObject(context.CommerceContext.GetObject<ProblemObjects>());
                newContext.CommerceContext.AddObject(context.CommerceContext.GetObject<AzureStorageService>());

                context.Logger.LogDebug($"{Name}-Exporting images for sellable item: '{entityId}'. Environment: {context.CommerceContext.Environment.Name}");
                await Commander.Pipeline<ExportSellableItemImagesPipeline>()
                    .RunAsync(
                        new ExportEntitiesArgument(entityId, arg),
                        newContext)
                    .ConfigureAwait(false);

                if (error)
                {
                    context.Abort(
                        await context.CommerceContext.AddMessage(
                            context.GetPolicy<KnownResultCodes>().Error,
                            AzureStorageConstants.Errors.ExportSellableItemsFailed,
                            new object[] { Name },
                            $"{Name}: Export images for sellable items failed.").ConfigureAwait(false),
                        context);
                }
            }

            context.Logger.LogInformation($"{Name}-Exporting images sellable items Completed: {(int)itemsProcessed}. Environment: {context.CommerceContext.Environment.Name}");
            return arg;
        }

        protected virtual async Task<FindEntitiesInListArgument> GetListIds<T>(CommercePipelineExecutionContext context, string listName, int take, int skip = 0)
        {
            var arg = new FindEntitiesInListArgument(typeof(T), listName, skip, take)
            {
                LoadEntities = false,
                LoadTotalItemCount = true
            };
            var result = await Commander.Pipeline<FindEntitiesInListPipeline>().RunAsync(arg, context.CommerceContext.PipelineContextOptions).ConfigureAwait(false);

            return result;
        }
    }
}