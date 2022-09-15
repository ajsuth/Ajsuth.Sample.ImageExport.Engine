// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageExportPolicy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Models;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Ajsuth.Sample.ImageExport.Engine.Policies
{
    /// <summary>Defines the image export policy.</summary>
    /// <seealso cref="Policy" />
    public class ImageExportPolicy : Policy
    {
        /// <summary>
        /// Gets or sets the export standalone products flag.
        /// </summary>
        /// <value>
        /// The export standalone products flag.
        /// </value>
        public bool ExportStandaloneProducts { get; set; } = false;

        /// <summary>
        /// Gets or sets the export product with variants flag.
        /// </summary>
        /// <value>
        /// The export product with variants flag.
        /// </value>
        public bool ExportProductsWithVariants { get; set; } = false;

        /// <summary>
        /// Gets or sets the upload images flag.
        /// </summary>
        /// <value>
        /// The upload images flag.
        /// </value>
        public bool UploadImages { get; set; } = false;

        /// <summary>
        /// Gets or sets the override existing images flag.
        /// </summary>
        /// <value>
        /// The override existing images flag.
        /// </value>
        public bool OverrideExistingImages { get; set; } = false;

        /// <summary>
        /// Gets or sets the local image location for downloaded images.
        /// </summary>
        /// <value>
        /// The local image location for downloaded images.
        /// </value>
        public string LocalImageLocation { get; set; }

        /// <summary>
        /// Gets or sets the image configurations.
        /// </summary>
        /// <value>
        /// The image configurations.
        /// </value>
        public List<ImageConfiguration> ImageConfigurations { get; set; } = new List<ImageConfiguration>();
    }
}
