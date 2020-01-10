using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ==============================================================================
 * 创建日期：2019/5/5 23:34:25
 * 创 建 者：wgd
 * 功能描述：SelectionHighlightFactory  
 * ==============================================================================*/
namespace Microsoft.VisualStudio.Extensions.TodoGlyph
{
    [TextViewRole("DOCUMENT")]
    [ContentType("text")]
    [Export(typeof(IWpfTextViewCreationListener))]
    internal sealed class SelectionHighlightFactory : IWpfTextViewCreationListener
    {
        [Import]
        internal ITextSearchService TextSearchService { get; set; }

        [Import]
        internal IViewTagAggregatorFactoryService AggregatorFactory { get; set; }

        [Order(After = "SelectionAndProvisionHighlight", Before = "Text")]
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("SelectionHighlight")]
        [TextViewRole("DOCUMENT")]
        public AdornmentLayerDefinition editorAdornmentLayer;

        public void TextViewCreated(IWpfTextView textView)
        {
            ITagAggregator<ToDoTag> tagAggregator = this.AggregatorFactory.CreateTagAggregator<ToDoTag>(textView);
            new SelectionHighlight(textView, this.TextSearchService, tagAggregator);
        }
    }
}
