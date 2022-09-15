// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExportImagesPipeline.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Models;
using Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines
{
    /// <summary>Defines the ExportImages pipeline interface</summary>
    /// <seealso cref="IPipeline{TInput, TOutput, TContext}" />
    [PipelineDisplayName(AzureStorageConstants.Pipelines.ExportImages)]
    public interface IExportImagesPipeline : IPipeline<ExportImagesArgument, ExportResult, CommercePipelineExecutionContext>
    {
    }
}
