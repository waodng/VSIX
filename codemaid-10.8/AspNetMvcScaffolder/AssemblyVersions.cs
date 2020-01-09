﻿using AspNetMvcScaffolder.Versions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AspNetMvcScaffolder
{
    internal static class AssemblyVersions
    {
        private const string AssemblyVersionsFile = @"Templates\AssemblyVersions.xml";
        private const string AssemblyElementXPath = "/Assemblies/Assembly";
        public static readonly string MvcAssemblyName = "System.Web.Mvc";
        public const string WebApiAssemblyName = "System.Web.Http";
        public const string ODataAssemblyName = "System.Web.OData";
        public static readonly Version WebApiAssemblyMinVersion = new Version(5, 0, 0);
        public static readonly Version WebApiAssemblyMaxVersion = new Version(6, 0, 0, 0);
        public static readonly Version ODataAssemblyMinVersion = new Version(5, 0, 0);
        public static readonly Version ODataAssemblyMaxVersion = new Version(6, 0, 0, 0);
        // The name and minimum version of Entity Framework required to use async controller actions
        public const string AsyncEntityFrameworkAssemblyName = "EntityFramework";
        public static readonly Version AsyncEntityFrameworkMinVersion = new Version(6, 0, 0);

        private static IDictionary<string, string> Versions
        {
            get;
            set;
        }

        /// <summary>
        /// This function retrieves the latest version information from the build file for a provided assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <returns>The latest version information for the specified assembly from the build file. If the version information for the
        /// specified assembly is not present in the assembly build file, then the function returns null.</returns>
        public static Version GetLatestAssemblyVersion(string assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }

            if (Versions == null)
            {
                Versions = VersionFileReader.GetVersions(AssemblyVersionsFile, AssemblyElementXPath);
                if (Versions == null)
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Resources.VersionsFileMissing, AssemblyVersionsFile));
                }
            }

            string assemblyVersion;
            Versions.TryGetValue(assemblyName, out assemblyVersion);

            Version version;
            if (!Version.TryParse(assemblyVersion, out version))
            {
                return null;
            }

            return version;
        }
    }
}
