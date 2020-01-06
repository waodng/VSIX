﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MadsKristensen.EditorExtensions.Settings;
using Microsoft.Html.Core;
using Microsoft.Html.Editor.Validation.Validators;
using Microsoft.Html.Validation;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Web.Editor;

namespace MadsKristensen.EditorExtensions.Html
{
    [Export(typeof(IHtmlElementValidatorProvider))]
    [ContentType(HtmlContentTypeDefinition.HtmlContentType)]
    public class FoundationClassValidatorProvider : BaseHtmlElementValidatorProvider<FoundationClassValidator>
    { }

    public class FoundationClassValidator : BaseValidator
    {
        private static string _errorMissingColumns = "Foundation: When using \"small-#\", \"medium-#\" or \"large-#\", you must also specify the class \"columns\".";
        private static string _errorMissingSize = "Foundation: When using \"columns\", you must also specify the class \"small-#\", \"medium-#\" or \"large-#\".";

        public override IList<IHtmlValidationError> ValidateElement(ElementNode element)
        {
            var results = new ValidationErrorCollection();

            if (!WESettings.Instance.Html.EnableFoundationValidation)
                return results;

            var classNames = element.GetAttribute("class");

            if (classNames == null)
                return results;

            if (!ColumnPairElementsOk(classNames.Value))
                results.AddAttributeError(element,
                                          classNames.Value.Contains("column") ? _errorMissingSize : _errorMissingColumns,
                                          HtmlValidationErrorLocation.AttributeValue,
                                          element.Attributes.IndexOf(classNames));

            return results;
        }

        public static bool ColumnPairElementsOk(string input)
        {
            var containColumnClass = input.Split(' ').Any(x => new[] { "columns", "column" }.Contains(x));
            var containSizeClass = new[] { "small-", "medium-", "large-" }
                                  .Any(x => input.Split(' ')
                                  .Where(toExclude => !toExclude.Contains("block-grid"))
                                  .Any(y => y.StartsWith(x, StringComparison.Ordinal)));

            // If both are there, or both are missing it's OK
            // Ok too if only column without size. The FoundationColumnsValidator will check if it's realy OK (only one column class).
            return containColumnClass && containSizeClass ||
                  !containColumnClass && !containSizeClass ||
                   containColumnClass && !containSizeClass;
        }
    }
}
