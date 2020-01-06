using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System;

namespace AspNetMvcScaffolder.Metadata
{
    [Serializable]
    public sealed class CodeModelModelMetadata : ModelMetadata
    {
        static Type[] bindableNonPrimitiveTypes = new[] { typeof(string), typeof(decimal), typeof(Guid), typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan), };

        public CodeModelModelMetadata(CodeType model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            Properties = GetModelProperties(model).ToArray();
            PrimaryKeys = GetModelProperties(model).Where(mp => mp.IsPrimaryKey).ToArray();
        }

        private static IList<PropertyMetadata> GetModelProperties(CodeType codeType)
        {
            IList<PropertyMetadata> results = new List<PropertyMetadata>();
            foreach (CodeProperty property in codeType.GetPublicMembers().OfType<CodeProperty>())
            {
                if (property.HasPublicGetter() && !property.IsIndexerProperty() && IsBindableType(property.Type))
                {
                    results.Add(new CodeModelPropertyMetadata(property));
                }
            }

            return results;
        }

        private static bool IsBindableType(CodeTypeRef type)
        {
            return type.IsPrimitiveType() || bindableNonPrimitiveTypes.Any(x => type.IsMatchForReflectionType(x));
        }
    }
}
