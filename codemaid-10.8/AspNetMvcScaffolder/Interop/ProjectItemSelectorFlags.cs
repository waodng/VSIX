using System;
namespace AspNetMvcScaffolder.Interop
{
    [Flags]
    internal enum ProjectItemSelectorFlags : int
    {
        PISF_ReturnAppRelativeUrls = 0, 
        PSIF_ReturnDocRelativeUrls = 1,
    }
}
