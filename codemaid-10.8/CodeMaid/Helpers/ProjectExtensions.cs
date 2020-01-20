using EnvDTE;
using System;
using System.Linq;
using System.IO;

namespace Waodng.CodeMaid.Helpers
{
    /// <summary>
    /// A set of extension methods for <see cref="Project" />.
    /// </summary>
    internal static class ProjectExtensions
    {
        
        /// <summary>
        /// 获取解决方案文件路径
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetRootFolder(this Project project)
        {
            if (project == null || string.IsNullOrEmpty(project.FullName))
                return null;

            string fullPath;

            try
            {
                fullPath = project.Properties.Item("FullPath").Value as string;
            }
            catch (ArgumentException)
            {
                try
                {
                    // MFC projects don't have FullPath, and there seems to be no way to query existence
                    fullPath = project.Properties.Item("ProjectDirectory").Value as string;
                }
                catch (ArgumentException)
                {
                    // Installer projects have a ProjectPath.
                    fullPath = project.Properties.Item("ProjectPath").Value as string;
                }
            }

            if (string.IsNullOrEmpty(fullPath))
                return File.Exists(project.FullName) ? Path.GetDirectoryName(project.FullName) : null;

            if (Directory.Exists(fullPath))
                return fullPath;

            if (File.Exists(fullPath))
                return Path.GetDirectoryName(fullPath);

            return null;
        }
    }
}