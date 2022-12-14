// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportEntitiesArgument.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;

namespace Ajsuth.Sample.ImageExport.Engine.Pipelines.Arguments
{
    /// <summary>Defines the ExportEntities pipeline argument.</summary>
    /// <seealso cref="PipelineArgument" />
    public class ExportEntitiesArgument : ExportImagesArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportEntitiesArgument"/> class.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        public ExportEntitiesArgument(string entityId, ExportImagesArgument exportArgument)
            : base(exportArgument?.ImageSettings, exportArgument?.AzureStorageSettings)
        {
            Condition.Requires(entityId, nameof(entityId)).IsNotNull();

            EntityId = entityId;
        }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        public string EntityId { get; set; }
    }
}