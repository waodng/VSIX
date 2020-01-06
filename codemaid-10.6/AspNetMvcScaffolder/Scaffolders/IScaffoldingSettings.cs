using AspNetMvcScaffolder.VisualStudio;

namespace AspNetMvcScaffolder
{
    public interface IScaffoldingSettings
    {
        void LoadSettings(IProjectSettings settings);

        void SaveSettings(IProjectSettings settings);
    }
}
