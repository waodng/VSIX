using EnvDTE;
using System;


namespace AspNetMvcScaffolder.VisualStudio
{
    public interface IVisualStudioIntegration
    {
        IEditorIntegration Editor
        {
            get;
        }

        /// <summary>
        /// Gets the Visual Studio Shell ServiceProvider.
        /// </summary>
        IServiceProvider ServiceProvider
        {
            get;
        }

        /// <summary>
        /// Gets a IProjectSettings instance for the given project, or null if the project does not implement
        /// IVsBuildPropertyStorage.
        /// </summary>
        /// <param name="project">The project</param>
        /// <returns>An IProjectSettings instance, or null.</returns>
        IProjectSettings GetProjectSettings(Project project);

        /// <summary>
        /// Shows an error message in a modal message box dialog.
        /// </summary>
        /// <param name="caption">The dialog caption.</param>
        /// <param name="message">The dialog message.</param>
        void ShowErrorMessage(string caption, string message);
    }
}
