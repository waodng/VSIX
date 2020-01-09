using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AspNetMvcScaffolder.VisualStudio
{
    public interface IEditorInterfaces
    {
        ITextBuffer TextBuffer
        {
            get;
        }

        ITextDocument TextDocument
        {
            get;
        }

        IVsTextBuffer VsTextBuffer
        {
            get;
        }
    }
}
