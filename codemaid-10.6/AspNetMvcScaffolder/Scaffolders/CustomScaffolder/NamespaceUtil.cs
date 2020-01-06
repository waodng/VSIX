using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Scaffolding;

namespace AspNetMvcScaffolder
{
    public class NamespaceUtil
    {
        public static string GetFullTypeNameNamespace(string fullTypeName)
        {
            if (string.IsNullOrWhiteSpace(fullTypeName))
            {
                return string.Empty;
            }

            string[] splits = fullTypeName.Split('.');

            splits = splits.Take(splits.Length - 1).ToArray();

            return string.Join(".", splits);
        }


        public static string GetNamespaceFolderPath(string nameNamespace)
        {
            if (string.IsNullOrWhiteSpace(nameNamespace))
            {
                return string.Empty;
            }

            string[] splits = nameNamespace.Split('.');

            return Path.Combine(splits);
        }

        public static string GetFullTypeNameShortTypeName(string fullTypeName)
        {
            if (string.IsNullOrWhiteSpace(fullTypeName))
            {
                return string.Empty;
            }

            return fullTypeName.Split('.').Last();
        }

        public static string GetFullTypeNameFolderPath(string fullTypeName)
        {
            return GetNamespaceFolderPath(GetFullTypeNameNamespace(fullTypeName));
        }


        public static string GetNamespaceFolderPath(Project project, string fullTypeName)
        {
            return GetNamespaceFolderPath(GetFullTypeNameNamespace(fullTypeName));
        }
    }
}
