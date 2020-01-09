using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ==============================================================================
 * 创建日期：2019/8/29 22:39:00
 * 创 建 者：wgd
 * 功能描述：WorkerLogic  
 * ==============================================================================*/
namespace SteveCadwallader.CodeMaid.Logic.NestIn
{
    /// <summary>
    /// 本方法用于文件嵌套，和移除文件嵌套显示功能
    /// </summary>
    public class WorkerLogic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DTE2 envDte;

       /// <summary>
       /// 
       /// </summary>
        private readonly IRootSelector selector;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="envDte"></param>
        /// <param name="selector"></param>
        public WorkerLogic(DTE2 envDte, IRootSelector selector)
        {
            this.envDte = envDte;
            this.selector = selector;
        }

        /// <summary>
        /// 嵌入文件夹
        /// </summary>
        public void Nest()
        {
            IEnumerable<FileItem> candidates = (from si in this.envDte.SelectedItems.OfType<SelectedItem>()
                                                select new FileItem
                                                {
                                                    Name = si.Name
                                                }).Reverse<FileItem>();
            FileItem root = this.selector.Select(candidates);
            if (root != null)
            {
                ProjectItem projectItem = this.envDte.SelectedItems.OfType<SelectedItem>().First((SelectedItem se) => se.Name == root.Name).ProjectItem;
                IEnumerable<ProjectItem> enumerable = from se in this.envDte.SelectedItems.OfType<SelectedItem>()
                                                      select se.ProjectItem into pi
                                                      where pi.Name != root.Name
                                                      select pi;
                foreach (ProjectItem projectItem2 in enumerable)
                {
                    projectItem.ProjectItems.AddFromFile(projectItem2.get_FileNames(0));
                }
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000029B8 File Offset: 0x00000BB8
        public void UnNest()
        {
            ProjectItem[] source = (from si in this.envDte.SelectedItems.OfType<SelectedItem>()
                                    select si.ProjectItem).ToArray<ProjectItem>();
            string[] selectedNames = (from pi in source
                                      select pi.Name).ToArray<string>();
            ProjectItem[] source2 = (from pi in source
                                     where pi.ProjectItems.OfType<ProjectItem>().Any((ProjectItem child) => selectedNames.Contains(child.Name))
                                     select pi).ToArray<ProjectItem>();
            Dictionary<ProjectItem, IEnumerable<ProjectItem>> dictionary = source2.ToDictionary((ProjectItem r) => r, (ProjectItem r) => from pi in r.ProjectItems.OfType<ProjectItem>()
                                                                                                                                         where selectedNames.Contains(pi.Name)
                                                                                                                                         select pi);
            foreach (KeyValuePair<ProjectItem, IEnumerable<ProjectItem>> keyValuePair in dictionary)
            {
                foreach (ProjectItem childItem in keyValuePair.Value)
                {
                    WorkerLogic.UnNest(keyValuePair.Key.ContainingProject, childItem);
                }
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002B2C File Offset: 0x00000D2C
        private static void UnNest(Project parent, ProjectItem childItem)
        {
            foreach (ProjectItem childItem2 in childItem.ProjectItems.OfType<ProjectItem>())
            {
                WorkerLogic.UnNest(parent, childItem2);
            }
            object value = childItem.Properties.Item("ItemType").Value;
            string text = childItem.get_FileNames(0);
            string tempFileName = Path.GetTempFileName();
            File.Copy(text, tempFileName, true);
            childItem.Delete();
            File.Copy(tempFileName, text);
            File.Delete(tempFileName);
            ProjectItem projectItem = parent.ProjectItems.AddFromFile(text);
            projectItem.Properties.Item("ItemType").Value = value;
        }
    }
}
