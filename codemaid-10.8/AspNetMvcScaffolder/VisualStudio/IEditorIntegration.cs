using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace AspNetMvcScaffolder.VisualStudio
{
    public interface IEditorIntegration
    {
        IEditorInterfaces GetOrOpenDocument(string path);

        IVsWindowFrame CreateAndOpenReadme(string text);

        void OpenFileInEditor(string filePath);

        void FormatDocument(string filePath);

        IDisposable SuppressChangeNotifications(string filePath);
    }
}
