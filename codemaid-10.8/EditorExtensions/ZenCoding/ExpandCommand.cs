using System;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Projection;
using ZenCodingNet;

namespace Waodng.CodeMaid.Integration.ZenCoding
{
    internal class ExpandCommand : BaseCommand
    {
        private static readonly Regex _bracket = new Regex(@"<([a-z0-9]*)\b[^>]*>([^<]*)</\1>", RegexOptions.IgnoreCase);
        private static readonly Regex _quotes = new Regex("(=\"()\")", RegexOptions.IgnoreCase);

        private ICompletionBroker _broker;
        private IWpfTextView _view;
        private ITextBufferUndoManager _undoManager;
        private IClassifier _classifier;
        private static Span _emptySpan = new Span();

        public ExpandCommand(IWpfTextView view, ICompletionBroker broker, ITextBufferUndoManagerProvider undoProvider, IClassifier classifier)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _view = view;
            _broker = broker;
            _undoManager = undoProvider.GetTextBufferUndoManager(view.TextBuffer);
            _classifier = classifier;
        }

        public override int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            //用户按tab键时
            if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB && !_broker.IsCompletionActive(_view))
            {
                if (InvokeZenCoding())
                {
                    return VSConstants.S_OK;
                }
            }

            return Next.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        /// <summary>
        /// 调用ZenCoding
        /// </summary>
        /// <returns></returns>
        private bool InvokeZenCoding()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string syntax = string.Empty;
            //获取用户输入的标签
            Span zenSpan = GetSyntaxSpan(out syntax);

            if (zenSpan.IsEmpty || _view.Selection.SelectedSpans[0].Length > 0 || !IsValidTextBuffer())
                return false;

            ////用户输入html语法标签
            //string syntax1 = _view.TextBuffer.CurrentSnapshot.GetText(zenSpan);

            var parser = new Parser();
            string result = parser.Parse(syntax, ZenType.HTML);

            if (!string.IsNullOrEmpty(result))
            {
                //撤销事务使用
                using (ITextUndoTransaction undo = _undoManager.TextBufferUndoHistory.CreateTransaction("ZenCoding"))
                {
                    ITextSelection selection = UpdateTextBuffer(zenSpan, result);

                    var newSpan = new Span(zenSpan.Start, selection.SelectedSpans[0].Length);
                    //选区内容格式化命令
                    EditorExtensions.EditorExtensionsPackage.DTE.ExecuteCommand("Edit.FormatSelection");
                    SetCaret(newSpan, false);

                    selection.Clear();
                    undo.Complete();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 分析输入标签是否合法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Span GetSyntaxSpan(out string result)
        {
            int position = _view.Caret.Position.BufferPosition.Position;
            result = string.Empty;
           
            if (position > 0)
            {
                var line = _view.TextBuffer.CurrentSnapshot.GetLineFromPosition(position);
                string text = line.GetText().TrimEnd();

                if (string.IsNullOrWhiteSpace(text) || text.Length < position - line.Start || text.Length + line.Start > position)
                    return _emptySpan;

                result = text.Substring(0, position - line.Start).TrimStart();

                if (result.Length > 0 && !text.Contains("<") && !char.IsWhiteSpace(result.Last()))
                {
                    return new Span(line.Start.Position + text.IndexOf(result, StringComparison.OrdinalIgnoreCase), result.Length);
                }
            }
            return _emptySpan;
        }
        private bool IsValidTextBuffer()
        {
            IProjectionBuffer projection = _view.TextBuffer as IProjectionBuffer;
            if (projection != null)
            {
                SnapshotPoint snapshotPoint = _view.Caret.Position.BufferPosition;

                System.Collections.Generic.IEnumerable<ITextBuffer> buffers = projection.SourceBuffers.Where(
                    s =>
                        !s.ContentType.IsOfType("html")
                        && !s.ContentType.IsOfType("htmlx")
                        && !s.ContentType.IsOfType("inert")
                        && !s.ContentType.IsOfType("CSharp")
                        && !s.ContentType.IsOfType("VisualBasic")
                        && !s.ContentType.IsOfType("RoslynCSharp")
                        && !s.ContentType.IsOfType("RoslynVisualBasic"));


                foreach (ITextBuffer buffer in buffers)
                {
                    SnapshotPoint? point = _view.BufferGraph.MapDownToBuffer(snapshotPoint, PointTrackingMode.Negative, buffer, PositionAffinity.Predecessor);

                    if (point.HasValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool SetCaret(Span zenSpan, bool isReverse)
        {
            string text = _view.TextBuffer.CurrentSnapshot.GetText();
            Span quote = FindTabSpan(zenSpan, isReverse, text, _quotes);
            Span bracket = FindTabSpan(zenSpan, isReverse, text, _bracket);

            if (bracket.Start > 0 && (quote.Start == 0 ||
                                      (!isReverse && (bracket.Start < quote.Start)) ||
                                      (isReverse && (bracket.Start > quote.Start))))
            {
                quote = bracket;
            }

            if (zenSpan.Contains(quote.Start))
            {
                MoveTab(quote);
                return true;
            }
            else if (!isReverse)
            {
                MoveTab(new Span(zenSpan.End, 0));
                return true;
            }

            return false;
        }

        private void MoveTab(Span quote)
        {
            _view.Caret.MoveTo(new SnapshotPoint(_view.TextBuffer.CurrentSnapshot, quote.Start));
        }

        private static Span FindTabSpan(Span zenSpan, bool isReverse, string text, Regex regex)
        {
            MatchCollection matches = regex.Matches(text);

            if (!isReverse)
            {
                foreach (Match match in matches)
                {
                    Group group = match.Groups[2];

                    if (group.Index >= zenSpan.Start)
                    {
                        return new Span(group.Index, group.Length);
                    }
                }
            }
            else
            {
                for (int i = matches.Count - 1; i >= 0; i--)
                {
                    Group group = matches[i].Groups[2];

                    if (group.Index < zenSpan.End)
                    {
                        return new Span(group.Index, group.Length);
                    }
                }
            }

            return _emptySpan;
        }

        private ITextSelection UpdateTextBuffer(Span zenSpan, string result)
        {
            _view.TextBuffer.Replace(zenSpan, result);

            var point = new SnapshotPoint(_view.TextBuffer.CurrentSnapshot, zenSpan.Start);
            var snapshot = new SnapshotSpan(point, result.Length);
            _view.Selection.Select(snapshot, false);

            return _view.Selection;
        }

        public override int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (pguidCmdGroup == VSConstants.VSStd2K && prgCmds[0].cmdID == (uint)VSConstants.VSStd2KCmdID.FORMATDOCUMENT)
            {
                prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_ENABLED | (uint)OLECMDF.OLECMDF_SUPPORTED;
                return VSConstants.S_OK;
            }

            return Next.QueryStatus(pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }
}
