using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of removing whitespace.
    /// </summary>
    internal class RemoveWhitespaceLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="RemoveWhitespaceLogic" /> class.
        /// </summary>
        private static RemoveWhitespaceLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="RemoveWhitespaceLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="RemoveWhitespaceLogic" /> class.</returns>
        internal static RemoveWhitespaceLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new RemoveWhitespaceLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveWhitespaceLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private RemoveWhitespaceLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Removes blank lines from the bottom of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAtBottom(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAtBottom) return;

            EditPoint cursor = textDocument.EndPoint.CreateEditPoint();
            cursor.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
        }

        /// <summary>
        /// 删除指定文本文档顶部的空白行。
        /// Removes blank lines from the top of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAtTop(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAtTop) return;

            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();
            cursor.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
        }

        /// <summary>
        /// 删除属性后的空行。
        /// Removes blank lines after attributes.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAfterAttributes(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAfterAttributes) return;

            const string pattern = @"(^[ \t]*\[[^\]]+\][ \t]*(//[^\r\n]*)*)(\r?\n){2}(?![ \t]*//)";
            string replacement = @"$1" + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// 移除大括号后的空白行。
        /// Removes blank lines after an opening brace.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAfterOpeningBrace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace) return;

            const string pattern = @"\{([ \t]*(//[^\r\n]*)*)(\r?\n){2,}";
            string replacement = @"{$1" + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// 删除右大括号前的空行。
        /// Removes blank lines before a closing brace.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesBeforeClosingBrace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace) return;

            const string pattern = @"(\r?\n){2,}([ \t]*)\}";
            string replacement = Environment.NewLine + @"$2}";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// 删除结束标记前的空行。
        /// Removes blank lines before a closing tag.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesBeforeClosingTag(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingTags) return;
            //正则表达式
            const string pattern = @"(\r?\n){2,}([ \t]*)</";
            //替换字符串
            string replacement = Environment.NewLine + @"$2</";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// 删除链接语句之间的空行。
        /// Removes blank lines between chained statements.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesBetweenChainedStatements(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesBetweenChainedStatements) return;

            const string pattern = @"(\r?\n){2,}([ \t]*)(else|catch|finally)( |\t|\r?\n)";
            string replacement = Environment.NewLine + @"$2$3$4";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// 删除右尖括号前的空白。
        /// Removes blank spaces before a closing angle bracket.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankSpacesBeforeClosingAngleBracket(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankSpacesBeforeClosingAngleBrackets) return;

            // Remove blank spaces before regular closing angle brackets.
            const string pattern = @"(\r?\n)*[ \t]+>\r?\n";
            string replacement = @">" + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);

            // Handle blank spaces before self closing angle brackets based on insert blank space setting.
            if (Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets)
            {
                const string oneSpacePattern = @"(\r?\n)*[ \t]{2,}/>\r?\n";
                string oneSpaceReplacement = @" />" + Environment.NewLine;

                TextDocumentHelper.SubstituteAllStringMatches(textDocument, oneSpacePattern, oneSpaceReplacement);
            }
            else
            {
                const string noSpacePattern = @"(\r?\n)*[ \t]+/>\r?\n";
                string noSpaceReplacement = @"/>" + Environment.NewLine;

                TextDocumentHelper.SubstituteAllStringMatches(textDocument, noSpacePattern, noSpaceReplacement);
            }
        }

        /// <summary>
        /// Removes the trailing newline from the end of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveEOFTrailingNewLine(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine) return;

            EditPoint cursor = textDocument.EndPoint.CreateEditPoint();

            if (cursor.AtEndOfDocument && cursor.AtStartOfLine && cursor.AtEndOfLine)
            {
                // Make an exception for C++ resource files to work-around known EOF issue: http://connect.microsoft.com/VisualStudio/feedback/details/173903/resource-compiler-returns-a-rc1004-unexpected-eof-found-error#details
                if (textDocument.GetCodeLanguage() == CodeLanguage.CPlusPlus &&
                    (textDocument.Parent.FullName.EndsWith(".h") || textDocument.Parent.FullName.EndsWith(".rc2")))
                {
                    return;
                }

                var backCursor = cursor.CreateEditPoint();
                backCursor.CharLeft();
                backCursor.Delete(cursor);
            }
        }

        /// <summary>
        /// 从指定的文本文档中删除所有行尾空白。
        /// Removes all end of line whitespace from the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveEOLWhitespace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveEndOfLineWhitespace) return;

            const string pattern = @"[ \t]+\r?\n";
            string replacement = Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// 从指定的文本文档中删除多个连续空行。
        /// Removes multiple consecutive blank lines from the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveMultipleConsecutiveBlankLines(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines) return;

            const string pattern = @"(\r?\n){3,}";
            string replacement = Environment.NewLine + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        #endregion Methods
    }
}