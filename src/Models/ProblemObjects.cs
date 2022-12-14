// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemObjects.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Ajsuth.Sample.ImageExport.Engine.Models
{
    /// <summary>Defines the ProblemObjects model.</summary>
    public class ProblemObjects
    {
        /// <summary>
        /// The list of products that have thrown errors/exceptions.
        /// </summary>
        public List<string> Products { get; set; } = new List<string>();
    }
}