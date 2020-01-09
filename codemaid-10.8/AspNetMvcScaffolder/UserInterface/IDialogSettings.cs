using AspNetMvcScaffolder.VisualStudio;

namespace AspNetMvcScaffolder.UserInterface
{
    /// <summary>
    /// An interface for Scaffolder Models that read and write dialog settings.
    /// </summary>
    public interface IDialogSettings
    {
        void LoadDialogSettings(IProjectSettings settings);

        void SaveDialogSettings(IProjectSettings settings);
    }
}
