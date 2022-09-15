// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportResult.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Commerce.Core;

namespace Ajsuth.Sample.ImageExport.Engine.Models
{
    /// <summary>Defines the ExportResult model.</summary>
    /// <seealso cref="Model" />
    public class ExportResult : Model
    {
        public ExportObject Products { get; set; } = new ExportObject();
        public ExportObject ProductImages { get; set; } = new ExportObject();
        public ExportObject VariantImages { get; set; } = new ExportObject();
    }
}