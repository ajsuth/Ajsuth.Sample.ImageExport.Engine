// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Ajsuth.Sample.ImageExport.Engine.Pipelines;
using Ajsuth.Sample.ImageExport.Engine.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Sitecore.Framework.Rules;
using System.Reflection;

namespace Ajsuth.Sample.ImageExport.Engine
{
    /// <summary>The configure sitecore class.</summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>The configure services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Rules(config => config.Registry(registry => registry.RegisterAssembly(assembly)));

            services.Sitecore().Pipelines(builder => builder

                .ConfigurePipeline<IConfigureOpsServiceApiPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.ConfigureOpsServiceApiBlock>()
                )

                .AddPipeline<IExportSellableItemImagesPipeline, ExportSellableItemImagesPipeline>(pipeline => pipeline
                    .Add<ValidateSellableItemBlock>()
                    .Add<ExportSellableItemImagesBlock>()
                )

                .AddPipeline<IExportImagesPipeline, ExportImagesPipeline>(pipeline => pipeline
                    .Add<PrepareExportBlock>()
                    .Add<PrepareAzureStorageBlock>()
                    .Add<ProcessSellableItemsBlock>()
                )

            );
        }
    }
}
