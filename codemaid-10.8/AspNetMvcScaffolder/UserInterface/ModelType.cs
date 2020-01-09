﻿using EnvDTE;
using System;
using System.Globalization;

namespace AspNetMvcScaffolder.UserInterface
{
    /// <summary>
    /// Wrapper for CodeType so we can use it in the UI.
    /// </summary>
    [Serializable]
    public class ModelType
    {
        public ModelType(CodeType codeType)
        {
            if (codeType == null)
            {
                throw new ArgumentNullException("codeType");
            }

            CodeType = codeType;
            TypeName = codeType.FullName;
            ShortTypeName = codeType.Name;
            DisplayName = (codeType.Namespace == null || String.IsNullOrWhiteSpace(codeType.Namespace.FullName)) ? codeType.Name: String.Format(CultureInfo.InvariantCulture, "{0} ({1})", codeType.Name, codeType.Namespace.FullName);
        }

        public ModelType(string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }

            CodeType = null;
            TypeName = typeName;
            DisplayName = typeName;
            ShortTypeName = NamespaceUtil.GetFullTypeNameShortTypeName(typeName);
        }

        public CodeType CodeType { get; set; }

        public string DisplayName { get; set; }

        public string TypeName { get; set; }

        public string ShortTypeName { get; set; }
    }
}
