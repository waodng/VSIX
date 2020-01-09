using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
//using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
//using Task = System.Threading.Tasks.Task;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    internal sealed class FilesDifferentCommand: BaseCommand
    {
        #region 单实例初始化

        public static FilesDifferentCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new FilesDifferentCommand(package);
            //监控配置文件决定是否启用该功能，不然无法显示按钮
            package.SettingsMonitor.Watch(s => s.Feature_FileDifferent, Instance.Switch);
        }

         internal FilesDifferentCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidFilesDifferent)
        {

        }


	    #endregion

        #region 私有方法

        /// <summary>
        /// 查询状态
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.Solution.IsOpen;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            if (CanFilesBeCompared(out string file1, out string file2))
            {
                if (!DiffFileUsingCustomTool(file1, file2))
                {
                    DiffFilesUsingDefaultTool(file1, file2);
                }
            }
        }

         private void DiffFilesUsingDefaultTool(string file1, string file2)
        {
            // This is the guid and id for the Tools.DiffFiles command
            string diffFilesCmd = "{5D4C0442-C0A2-4BE8-9B4D-AB1C28450942}";
            int diffFilesId = 256;
            object args = $"\"{file1}\" \"{file2}\"";

            Package.IDE.Commands.Raise(diffFilesCmd, diffFilesId, ref args, ref args);
        }

        //Visual Studio allows replacing the default diff tool with a custom one.
        //See, for example:
        //Using WinMerge: https://blog.paulbouwer.com/2010/01/31/replace-diffmerge-tool-in-visual-studio-team-system-with-winmerge/
        //Using BeyondCompare: http://stackoverflow.com/questions/4466238/how-to-configure-visual-studio-to-use-beyond-compare
        private bool DiffFileUsingCustomTool(string file1, string file2)
        {
            try
            {
                //Checking the registry to see if a custom tool is configured
                //Relevant information: https://social.msdn.microsoft.com/Forums/vstudio/en-US/37a26013-2f78-4519-85e5-d896ac27f31e/see-what-default-visual-studio-tfexe-compare-tool-is-set-to-using-visual-studio-api?forum=vsx
                string registryFolder = $"{Package.IDE.RegistryRoot}\\TeamFoundation\\SourceControl\\DiffTools\\.*\\Compare";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryFolder))
                {
                    string command = key?.GetValue("Command") as string;
                    if (string.IsNullOrEmpty(command)) return false;

                    string args = key.GetValue("Arguments") as string;
                    if (string.IsNullOrEmpty(args)) return false;

                    //Understanding the arguments: https://msdn.microsoft.com/en-us/library/ms181446(v=vs.100).aspx
                    args =
                        args.Replace("%1", $"\"{file1}\"")
                            .Replace("%2", $"\"{file2}\"")
                            .Replace("%5", string.Empty)
                            .Replace("%6", $"\"{file1}\"")
                            .Replace("%7", $"\"{file2}\"");
                    System.Diagnostics.Process.Start(command, args);
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                return false;
            }
        }

        //文件是否能比较
        private bool CanFilesBeCompared(out string file1, out string file2)
        {
            //获取选中比较文件
            IEnumerable<string> items = GetSelectedFiles();

            file1 = items.ElementAtOrDefault(0);
            file2 = items.ElementAtOrDefault(1);

            if (items.Count() == 1)
            {
                var dialog = new System.Windows.Forms.OpenFileDialog();
                dialog.InitialDirectory = Path.GetDirectoryName(file1);
                dialog.ShowDialog();

                file2 = dialog.FileName;
            }

            return !string.IsNullOrEmpty(file1) && !string.IsNullOrEmpty(file2)&& !(file1 == file2);
        }

        //获取选择的文件
        public IEnumerable<string> GetSelectedFiles()
        {
            var items = (Array)Package.IDE.ToolWindows.SolutionExplorer.SelectedItems;

            return from item in items.Cast<UIHierarchyItem>()
                   let pi = item.Object as ProjectItem
                   select pi.FileNames[1];
        }
	    #endregion
    }
}
