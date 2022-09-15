// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportImagesPipeline.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Models;
using Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines
{
    /// <summary>Defines the ExportImages pipeline.</summary>
    /// <seealso cref="CommercePipeline{TArg, TResult}" />
    /// <seealso cref="IExportImagesPipeline" />
    public class ExportImagesPipeline : CommercePipeline<ExportImagesArgument, ExportResult>, IExportImagesPipeline
    {
        /// <summary>Initializes a new instance of the <see cref="ExportImagesPipeline" /> class.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ExportImagesPipeline(IPipelineConfiguration<IExportImagesPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

