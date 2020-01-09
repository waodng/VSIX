﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace AspNetMvcScaffolder
{
    internal static class AddDependencyUtil
    {
        private const string OptimizationNamespace = "System.Web.Optimization";

        /// <summary>
        /// This function is used to verify if the specified text is present in the specified file.
        /// </summary>
        /// <param name="fileFullPath">The full path of the file including the filename and extension.</param>
        /// <param name="searchText">The text to be searched.</param>
        /// <returns><see langword="true" /> if the specified file is accessible and contains the specified text; 
        /// otherwise, <see langword="false" />
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We don't want to fail scaffolding if we can't read the file.")]
        public static bool IsSearchTextPresent(string fileFullPath, string searchText)
        {
            if (fileFullPath == null)
            {
                throw new ArgumentNullException("fileFullPath");
            }

            if (searchText == null)
            {
                throw new ArgumentNullException("searchText");
            }

            // This is currently used to check if the BundleConfig exists if the user upgrades from minimal dependency 
            // to full dependency or uses the add dependency scaffolder multiple times. If for any reason the BundleConfig is
            // inaccessible, the function returns false and new BundleConfig will be created which can be deleted by the user if not needed. 
            try
            {
                string text = File.ReadAllText(fileFullPath);
                return text.Contains(searchText);
            }
            catch
            {
                return false;
            }
        }
    }
}
