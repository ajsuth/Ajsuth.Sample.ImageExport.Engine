// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommerceOpsController.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Sample.ImageExport.Engine.Commands;
using Ajsuth.Sample.ImageExport.Engine.Models;
using Ajsuth.Sample.ImageExport.Engine.Policies;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajsuth.Sample.ImageExport.Engine.Controllers
{
    /// <summary>Defines the commerce ops controller</summary>
    /// <seealso cref="CommerceODataController" />
    public class CommerceOpsController : CommerceODataController
    {
        /// <summary>Initializes a new instance of the <see cref="CommerceOpsController" /> class.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public CommerceOpsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        /// <summary>
        /// Exports XC data to OrderCloud.
        /// </summary>
        /// <param name="value">The action parameters.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [ODataRoute("ExportImages", RouteName = CoreConstants.CommerceOpsApi)]
        public async Task<IActionResult> ExportImages([FromBody] ODataActionParameters value)
        {
            Condition.Requires(value, nameof(value)).IsNotNull();

            if (!ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (!value.ContainsKey("imageSettings") || !(value["imageSettings"] is ImageExportPolicy productSettings))
            {
                return new BadRequestObjectResult(value);
            }

            var command = Command<ExportImagesCommand>();
            var result = await command.Process(
                CurrentContext,
                productSettings,
                value["azureStorageSettings"] as AzureStoragePolicy).ConfigureAwait(false);

            return new ObjectResult(result);
        }
    }
}