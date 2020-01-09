using EnvDTE;

namespace FormatDocumentOnSave
{
    public interface IDocumentFilter
    {
        bool IsAllowed(Document document);
    }
}