﻿using EnvDTE;
using EnvDTE80;
using System;

namespace AspNetMvcScaffolder
{
    internal static class CodeTypeFilter
    {
        /// <summary>
        /// This function is used to verify if the specified <see cref="CodeType"/> is a valid class and if the class 
        /// contains an import statement for the specified namespace.
        /// </summary>
        /// <param name="codeType">The specified <see cref="CodeType"/>.</param>
        /// <param name="productNamespace">The specified namespace value.</param>
        /// <returns><see langword="true" /> if the <see cref="CodeType"/> contains the specified namespace import statement; 
        /// otherwise, <see langword="false" />
        /// </returns>
        /// <remarks>
        // The function will not identify imports using the type aliases. For example, the function would return false for "System.Web.Mvc"
        // if the imports are used as below:
        // using A1 = System;
        // using A1.Web.Mvc;
        /// </remarks>
        public static bool IsProductNamespaceImported(CodeType codeType, string productNamespace)
        {
            if (codeType == null)
            {
                throw new ArgumentNullException("codeType");
            }

            if (productNamespace == null)
            {
                throw new ArgumentNullException("productNamespace");
            }

            FileCodeModel codeModel = codeType.ProjectItem.FileCodeModel;
            if (codeModel != null)
            {
                foreach (CodeElement codeElement in codeModel.CodeElements)
                {
                    // This is needed to verify if the namespace import is present at the file level.
                    if (IsNamespaceImportPresent(codeElement, productNamespace))
                    {
                        return true;
                    }
                    // This is needed to verify if the import is present at the namespace level.
                    if (codeElement.Kind.Equals(vsCMElement.vsCMElementNamespace) && IsImportPresentUnderNamespace(codeElement, productNamespace))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsNamespaceImportPresent(CodeElement codeElement, string productNamespace)
        {
            if (codeElement.Kind.Equals(vsCMElement.vsCMElementImportStmt))
            {
                CodeImport codeImport = (CodeImport)codeElement;
                return String.Equals(codeImport.Namespace, productNamespace, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private static bool IsImportPresentUnderNamespace(CodeElement codeElement, string productNamespace)
        {
            foreach (CodeElement codeElementChildren in codeElement.Children)
            {
                if (IsNamespaceImportPresent(codeElementChildren, productNamespace))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
