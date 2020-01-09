using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using System;
using System.IO;

namespace AspNetMvcScaffolder
{
    internal static class TemplateSearchDirectories
    {
        public static string InstalledTemplateRoot
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(typeof(TemplateSearchDirectories).Assembly.Location), "Templates");
            }
        }

        public static string GetProjectTemplateRoot(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return Path.Combine(project.GetFullPath(), "CodeTemplates");
        }
    }
}
