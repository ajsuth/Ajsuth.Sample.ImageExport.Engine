// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportSellableItemImagesPipeline.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines
{
    /// <summary>Defines the ExportSellableItems pipeline.</summary>
    /// <seealso cref="CommercePipeline{TArg, TResult}" />
    /// <seealso cref="IExportSellableItemImagesPipeline" />
    public class ExportSellableItemImagesPipeline : CommercePipeline<ExportEntitiesArgument, SellableItem>, IExportSellableItemImagesPipeline
    {
        /// <summary>Initializes a new instance of the <see cref="ExportSellableItemImagesPipeline" /> class.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ExportSellableItemImagesPipeline(IPipelineConfiguration<IExportSellableItemImagesPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

