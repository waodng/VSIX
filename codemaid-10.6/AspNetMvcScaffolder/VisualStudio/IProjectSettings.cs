namespace AspNetMvcScaffolder.VisualStudio
{
    public interface IProjectSettings
    {
        string this[string key]
        {
            get;
            set;
        }
    }
}
