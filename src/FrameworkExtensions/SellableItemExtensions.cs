// <copyright file="SellableItemExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Commerce.Plugin.Catalog;
using System.Collections.Generic;

namespace Ajsuth.Sample.ImageExport.Engine.FrameworkExtensions
{
    /// <summary>
    /// Defines extensions for <see cref="SellableItem"/>
    /// </summary>
    public static class SellableItemExtensions
    {
        /// <summary>
        /// Determines if Variants will need to be created for the OrderCloud product. Sellable items with a single variation may be folded into a standalone product
        /// if the variation does not have any variation properties configured.
        /// </summary>
        /// <param name="sellableItem">The <see cref="SellableItem"/>.</param>
        /// <returns>True if there is more than 1 variant or if the sole variant does not have any variation properties configured.</returns>
        public static bool RequiresVariants(this SellableItem sellableItem)
        {
            if (sellableItem == null || !sellableItem.HasComponent<ItemVariationsComponent>())
            {
                return false;
            }

            var variationsComponent = sellableItem.GetComponent<ItemVariationsComponent>();
            var variations = variationsComponent.GetChildComponents<ItemVariationComponent>();
            if (variations.Count > 1)
            {
                return true;
            }

            var variation = variations[0];
            if (!variation.HasChildComponent<DisplayPropertiesComponent>())
            {
                return false;
            }

            var displayProperties = variation.GetChildComponent<DisplayPropertiesComponent>();

            return !string.IsNullOrWhiteSpace(displayProperties.Color) || !string.IsNullOrWhiteSpace(displayProperties.Size);
        }

        public static List<ItemVariationComponent> GetVariations(this SellableItem sellableItem)
        {
            if (!sellableItem.HasComponent<ItemVariationsComponent>())
            {
                return new List<ItemVariationComponent>();
            }

            return sellableItem.GetComponent<ItemVariationsComponent>().Variations;
        }
    }
}
