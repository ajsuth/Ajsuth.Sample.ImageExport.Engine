// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportToOrderCloudCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Models;
using Ajsuth.Sample.ImageExport.Engine.Pipelines;
using Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments;
using Ajsuth.Sample.ImageExport.Engine.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajsuth.Sample.ImageExport.Engine.Commands
{
    /// <summary>Defines the ExportToOrderCloud command.</summary>
    public class ExportImagesCommand : CommerceCommand
    {
        /// <summary>Gets or sets the commander.</summary>
        /// <value>The commander.</value>
        protected CommerceCommander Commander { get; set; }

        /// <summary>Initializes a new instance of the <see cref="ExportImagesCommand" /> class.</summary>
        /// <param name="commander">The <see cref="CommerceCommander"/>.</param>
        /// <param name="serviceProvider">The service provider</param>
        public ExportImagesCommand(CommerceCommander commander, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Commander = commander;
        }

        /// <summary>The process of the command.</summary>
        /// <param name="commerceContext">The commerce context.</param>
        /// <param name="exportSettings">The export settings.</param>
        /// <param name="siteSettings">The list of site settings.</param>
        /// <param name="productSettings">The product settings.</param>
        /// <param name="azureStorageSettings">The Azure Storage settings.</param>
        /// <returns>The <see cref="ExportResult"/>.</returns>
        public virtual async Task<ExportResult> Process(
            CommerceContext commerceContext,
            ImageExportPolicy productSettings,
            AzureStoragePolicy azureStorageSettings)
        {
            var context = commerceContext.CreatePartialClone();
            using (var activity = CommandActivity.Start(context, this))
            {
                var arg = new ExportImagesArgument(productSettings, azureStorageSettings);
                await Commander.Pipeline<IExportImagesPipeline>().RunAsync(arg, context.PipelineContextOptions).ConfigureAwait(false);
                
                return context.GetObject<ExportResult>();
            }
        }
    }
}