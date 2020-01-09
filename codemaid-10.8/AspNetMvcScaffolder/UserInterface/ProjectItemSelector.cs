using AspNetMvcScaffolder.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using AspNetMvcScaffolder;

namespace AspNetMvcScaffolder.UserInterface
{
    internal static class ProjectItemSelector
    {
        public static bool TrySelectItem(IVsHierarchy hierarchy, string title, string filter, string preselectedItem, out string relativePath)
        {
            if (hierarchy == null)
            {
                throw new ArgumentNullException("hierarchy");
            }
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            bool flag;
            int hr = ProjectItemSelector.SelectItem(hierarchy, filter, title, preselectedItem, out relativePath, out flag);
            return NativeMethods.Succeeded(hr) && !flag;
        }
        private static int SelectItem(IVsHierarchy hierarchy, string filter, string title, string preselectedItem, out string appRelUrlOfSelectedItem, out bool canceled)
        {
            appRelUrlOfSelectedItem = null;
            canceled = false;
            int num = -2147467259;
            if (hierarchy != null)
            {
                Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider = null;
                num = hierarchy.GetSite(out serviceProvider);
                if (NativeMethods.Succeeded(num) && serviceProvider != null)
                {
                    IProjectItemSelector projectItemSelector = serviceProvider.CreateSitedInstance<IProjectItemSelector>(typeof(IProjectItemSelector_Class).GUID);
                    if (projectItemSelector != null)
                    {
                        num = projectItemSelector.SelectItem(hierarchy, 4294967295u, filter, title, ProjectItemSelectorFlags.PISF_ReturnAppRelativeUrls, null, preselectedItem, null, out appRelUrlOfSelectedItem, out canceled);
                    }
                }
            }
            return num;
        }
    }
}
