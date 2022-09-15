// <copyright file="ExportImagesArgument.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments
{
    /// <summary>Defines the ExportImages pipeline argument.</summary>
    /// <seealso cref="PipelineArgument" />
    public class ExportImagesArgument : PipelineArgument
    {
        public ExportImagesArgument(ImageExportPolicy imageSettings, AzureStoragePolicy azureStorageSettings)
        {
            Condition.Requires(imageSettings, nameof(imageSettings)).IsNotNull();

            ImageSettings = imageSettings;
            AzureStorageSettings = azureStorageSettings;
        }

        /// <summary>
        /// The image settings
        /// </summary>
        public ImageExportPolicy ImageSettings { get; set; }

        /// <summary>
        /// The Azure Storage settings
        /// </summary>
        public AzureStoragePolicy AzureStorageSettings { get; set; }
    }
}
