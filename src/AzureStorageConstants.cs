// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureStorageConstants.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ajsuth.Sample.ImageExport.Engine
{
    /// <summary>The AzureStorage constants.</summary>
    public class AzureStorageConstants
    {
        /// <summary>
        /// The names of the errors.
        /// </summary>
        public static class Errors
        {
            /// <summary>
            /// The export sellable items failed error name.
            /// </summary>
            public const string ExportSellableItemsFailed = nameof(ExportSellableItemsFailed);
            
            /// <summary>
            /// The get product failed error name.
            /// </summary>
            public const string GetProductFailed = nameof(GetProductFailed);
        }

        /// <summary>
        /// The names of the lists.
        /// </summary>
        public static class Lists
        {
            /// <summary>
            /// The sellable items list name.
            /// </summary>
            public const string SellableItems = nameof(SellableItems);
        }

        /// <summary>
        /// The names of the pipelines.
        /// </summary>
        public static class Pipelines
        {
            /// <summary>
            /// The export sellable item images pipeline name.
            /// </summary>
            public const string ExportSellableItemImages = "AzureStorage.Pipeline.ExportSellableItemImages";

            /// <summary>
            /// The export images pipeline name.
            /// </summary>
            public const string ExportImages = "AzureStorage.Pipeline.ExportImages";

            /// <summary>
            /// The names of the pipeline blocks.
            /// </summary>
            public static class Blocks
            {
                /// <summary>
                /// The configure ops service api pipeline block name.
                /// </summary>
                public const string ConfigureOpsServiceApi = "AzureStorage.Block.ConfigureOpsServiceApi";

                /// <summary>
                /// The export sellable item images pipeline block name.
                /// </summary>
                public const string ExportSellableItemImages = "AzureStorage.Block.ExportSellableItemImages";

                /// <summary>
                /// The process sellable items pipeline block name.
                /// </summary>
                public const string ProcessSellableItems = "AzureStorage.Block.ProcessSellableItems";

                /// <summary>
                /// The prepare export pipeline block name.
                /// </summary>
                public const string PrepareExport = "AzureStorage.Block.PrepareExport";

                /// <summary>
                /// The validate sellable item pipeline block name.
                /// </summary>
                public const string ValidateSellableItem = "AzureStorage.Block.ValidateSellableItem";
            }
        }
    }
}