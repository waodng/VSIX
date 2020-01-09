//***************************************************************************
// 
//    Copyright (c) Microsoft Corporation. All rights reserved.
//    This code is licensed under the Visual Studio SDK license terms.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//***************************************************************************

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;

namespace Outline.TodoGlyph
{
    /// <summary>
    /// Empty ToDoTag class.
    /// </summary>
    internal class ToDoTag : IGlyphTag
    {
        public string HightText { get; set; }
    }

    /// <summary>
    /// This class implements ITagger for ToDoTag.  It is responsible for creating
    /// ToDoTag TagSpans, which our GlyphFactory will then create glyphs for.
    /// </summary>
    internal class SelectionTagger : ITagger<ToDoTag>
    {
        private IClassifier _aggregator;
        private ITextView _view;

        private ITextSelection _selection;

        private ITextBuffer _sourceBuffer;

        private string selectedText = "";

        private VirtualSnapshotSpan selectedWord;

        private ITextSearchService _textSearchService;

        private List<SnapshotSpan> glyphsToPlace;

        public SelectionTagger(ITextView view, ITextBuffer SourceBuffer, IClassifier aggregator, ITextSearchService TextSearchService)
        {
            this._view = view;
            this._selection = view.Selection;
            this._aggregator = aggregator;
            this._sourceBuffer = SourceBuffer;
            this._textSearchService = TextSearchService;
            this.glyphsToPlace = new List<SnapshotSpan>();
            this._selection.SelectionChanged += new EventHandler(this.OnSelectionChanged);
            this._view.LayoutChanged += this.OnLayoutChanged;
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            lock (SelectionHighlight.updateLock)
            {
                EventHandler<SnapshotSpanEventArgs> tagsChanged = this.TagsChanged;
                this.selectedText = this._view.Selection.StreamSelectionSpan.GetText();
                this.selectedWord = this._view.Selection.StreamSelectionSpan;
                this.glyphsToPlace.Clear();
                if (tagsChanged != null)
                {
                    tagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(this._sourceBuffer.CurrentSnapshot, 0, this._sourceBuffer.CurrentSnapshot.Length)));
                }
                if (!string.IsNullOrEmpty(this.selectedText) && !string.IsNullOrWhiteSpace(this.selectedText))
                {
                    int length = this.selectedText.Length;
                    int position = this._view.Selection.StreamSelectionSpan.Start.Position.Position;
                    int num = position + length;
                    if (position - 1 < 0 || !char.IsLetterOrDigit(this._view.TextSnapshot[position - 1]))
                    {
                        if (num >= this._view.TextSnapshot.GetText().Length || !char.IsLetterOrDigit(this._view.TextSnapshot[num]))
                        {
                            foreach (char c in this.selectedText)
                            {
                                if (!char.IsLetterOrDigit(c) && c != '_')
                                {
                                    return;
                                }
                            }
                            this.FindWordsInDocument();
                            if (tagsChanged != null)
                            {
                                tagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(this._sourceBuffer.CurrentSnapshot, 0, this._sourceBuffer.CurrentSnapshot.Length)));
                            }
                        }
                    }
                }
            }
        }

        private void FindWordsInDocument()
        {
            lock (SelectionHighlight.updateLock)
            {
                FindData findData = new FindData(this.selectedWord.GetText(), this.selectedWord.Snapshot);
                findData.FindOptions = FindOptions.WholeWord;
                this.glyphsToPlace.AddRange(this._textSearchService.FindAll(findData));
            }
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            EventHandler<SnapshotSpanEventArgs> tagsChanged = this.TagsChanged;
            if (tagsChanged != null)
            {
                tagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(this._sourceBuffer.CurrentSnapshot, 0, this._sourceBuffer.CurrentSnapshot.Length)));
            }
        }

        /// <summary>
        /// This method creates ToDoTag TagSpans over a set of SnapshotSpans.
        /// </summary>
        /// <param name="spans">A set of spans we want to get tags for.</param>
        /// <returns>The list of ToDoTag TagSpans.</returns>
        IEnumerable<ITagSpan<ToDoTag>> ITagger<ToDoTag>.GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (this.glyphsToPlace.Count == 0)
                yield break;

            foreach (SnapshotSpan span in this.glyphsToPlace)
            {
                yield return new TagSpan<ToDoTag>(span, new ToDoTag() { HightText = span.GetText().ToLower() });
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
