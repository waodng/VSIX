using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.NuGet;

namespace AspNetMvcScaffolder.VisualStudio
{
    public interface INuGetRepository
    {
        NuGetPackage GetPackage(CodeGenerationContext context, string id);
        string GetPackageVersion(CodeGenerationContext context, string id);
    }
}
