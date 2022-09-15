// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportSellableItemBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.FrameworkExtensions;
using Ajsuth.Sample.ImageExport.Engine.Models;
using Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments;
using Ajsuth.Sample.ImageExport.Engine.Policies;
using Ajsuth.Sample.ImageExport.Engine.Service;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines.Blocks
{
    /// <summary>Defines the asynchronous executing ExportSellableItemImages pipeline block</summary>
    /// <seealso cref="AsyncPipelineBlock{TInput, TOutput, TContext}" />
    [PipelineDisplayName(AzureStorageConstants.Pipelines.Blocks.ExportSellableItemImages)]
    public class ExportSellableItemImagesBlock : AsyncPipelineBlock<SellableItem, SellableItem, CommercePipelineExecutionContext>
    {
        /// <summary>Gets or sets the commerce commander.</summary>
        protected CommerceCommander Commander { get; set; }

        /// <summary>The export result model.</summary>
        protected ExportResult Result { get; set; }

        /// <summary>The problem objects model.</summary>
        protected ProblemObjects ProblemObjects { get; set; }

        /// <summary>The image settings.</summary>
        protected ImageExportPolicy ImageSettings { get; set; }

        /// <summary>Initializes a new instance of the <see cref="ExportSellableItemImagesBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public ExportSellableItemImagesBlock(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        /// <summary>Executes the pipeline block's code logic.</summary>
        /// <param name="arg">The pipeline argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="SellableItem"/>.</returns>
        public override async Task<SellableItem> RunAsync(SellableItem sellableItem, CommercePipelineExecutionContext context)
        {
            Condition.Requires(sellableItem).IsNotNull($"{Name}: The sellable item can not be null");

            Result = context.CommerceContext.GetObject<ExportResult>();
            ProblemObjects = context.CommerceContext.GetObject<ProblemObjects>();

            var exportSettings = context.CommerceContext.GetObject<ExportEntitiesArgument>();
            ImageSettings = exportSettings.ImageSettings;

            var requiresVariants = sellableItem.RequiresVariants();

            if (!ImageSettings.ExportStandaloneProducts && !requiresVariants
                || !ImageSettings.ExportProductsWithVariants && requiresVariants)
            {
                Result.Products.ItemsSkipped++;

                context.Abort(
                    await context.CommerceContext.AddMessage(
                        context.GetPolicy<KnownResultCodes>().Information,
                        "ProductTypeNotSupported",
                        new object[] { sellableItem.Id },
                        $"Ok| Sellable Item {sellableItem.Id} is of type {(requiresVariants ? "'product with variants'" : "'standalone product'")}").ConfigureAwait(false),
                    context);

                return null;
            }

            // 1. Create Product Images
            await CreateOrUpdateImages(context, sellableItem.GetComponent<ImagesComponent>(), sellableItem.FriendlyId);

            // 2. Create Variation Images
            var variations = sellableItem.GetVariations();
            foreach (var variation in variations)
            {
                await CreateOrUpdateImages(context, variation.GetChildComponent<ImagesComponent>(), sellableItem.FriendlyId, variation.Id);
            }

            return sellableItem;
        }

        /// <summary>
        /// Creates of Updates Images.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="imagesComponent">The <see cref="ImagesComponent"/>.</param>
        /// <param name="imageFolder">The relative folder where the images will be stored.</param>
        protected async Task CreateOrUpdateImages(CommercePipelineExecutionContext context, ImagesComponent imagesComponent, string imageFolder, string imageFolder2 = null)
        {
            var sitecoreConnectionPolicy = context.GetPolicy<SitecoreConnectionPolicy>();
            var storageService = context.CommerceContext.GetObject<AzureStorageService>();

            foreach (var imageId in imagesComponent.Images)
            {
                Result.ProductImages.ItemsProcessed++;
                var itemModel = await Commander.Pipeline<IGetItemByIdPipeline>().RunAsync(new ItemModelArgument(imageId), context).ConfigureAwait(false);
                if (itemModel == null)
                {
                    Result.ProductImages.ItemsErrored++;
                    context.Logger.LogError($"{Name}: Processing image '{imageId}' for '{imageFolder}' failed.");

                    continue;
                }

                var imageUrl = new Uri($"{sitecoreConnectionPolicy.MediaLibraryUrl}/{itemModel[ItemModel.MedialUrl]}");

                if (!ImageSettings.UploadImages)
                {
                    // download to file system
                    using (var client = new WebClient())
                    {
                        string localFilePath = Path.Combine(ImageSettings.LocalImageLocation, imageFolder);
                        if (!string.IsNullOrWhiteSpace(imageFolder2))
                        {
                            localFilePath = Path.Combine(localFilePath, imageFolder2);
                        }

                        if (!Directory.Exists(localFilePath))
                        {
                            Directory.CreateDirectory(localFilePath);
                        }

                        localFilePath += Path.Combine(localFilePath, imageUrl.Segments.Last());
                        if (ImageSettings.OverrideExistingImages && File.Exists(localFilePath))
                        {
                            Result.ProductImages.ItemsSkipped++;
                            context.Logger.LogInformation($"Image already exists at '{localFilePath}'");

                            continue;
                        }

                        client.DownloadFileAsync(imageUrl, localFilePath);
                        context.Logger.LogInformation($"Downloading image: {imageFolder}\\{imageFolder2}\\{imageUrl.Segments.Last()}");
                        Result.ProductImages.ItemsCreated++;
                    }
                }
                else
                {
                    var imageFolderPath = string.IsNullOrWhiteSpace(imageFolder2) ? imageFolder : $"{imageFolder}/{imageFolder2}";
                    var filePath = $"{imageFolderPath}/{itemModel[ItemModel.ItemName]}";
                    
                    using (var client = new WebClient())
                    {
                        var content = client.DownloadData(imageUrl);
                        using (var mem = new MemoryStream(content))
                        {
                            using (var image = System.Drawing.Image.FromStream(mem))
                            {
                                var uploadTasks = new List<Task>();
                                foreach (var imageConfig in ImageSettings.ImageConfigurations)
                                {
                                    var imageFilePath = $"{filePath}{imageConfig.FileNameSuffix}";
                                    if (ImageSettings.OverrideExistingImages && await storageService.FileExists(imageFilePath))
                                    {
                                        Result.ProductImages.ItemsSkipped++;
                                        context.Logger.LogInformation($"Image already exists at '{imageFilePath}'");

                                        continue;
                                    }

                                    var resizedImage = imageConfig.TargetSize != null ? image.ResizeSmallerDimensionToTarget((int)imageConfig.TargetSize) : image;
                                    var uploadTask = storageService.Save($"{imageFilePath}", resizedImage.ToBytes(ImageFormat.Png), "image/png");
                                    uploadTasks.Add(uploadTask);

                                    context.Logger.LogInformation($"Uploading image: {imageFilePath}");
                                    Result.ProductImages.ItemsCreated++;
                                }

                                await Task.WhenAll(uploadTasks);
                            }
                        }
                    }
                }
            }
        }
    }
}