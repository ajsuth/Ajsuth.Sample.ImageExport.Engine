// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExportSellableItemImagesPipeline.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines
{
    /// <summary>Defines the ExportSellableItemImages pipeline interface</summary>
    /// <seealso cref="IPipeline{TInput, TOutput, TContext}" />
    [PipelineDisplayName(AzureStorageConstants.Pipelines.ExportSellableItemImages)]
    public interface IExportSellableItemImagesPipeline : IPipeline<ExportEntitiesArgument, SellableItem, CommercePipelineExecutionContext>
    {
    }
}
