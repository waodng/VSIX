using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.NestIn;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up
    /// code in the selected documents.
    /// </summary>
    internal sealed class RefactoringCodeCommand : BaseCommand
    {
        #region Singleton

        public static RefactoringCodeCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new RefactoringCodeCommand(package);
            //监控配置文件决定是否启用该功能
            package.SettingsMonitor.Watch(s => s.Refactoring, Instance.Switch);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="NestInFileCommand" /> class.
        /// </summary>
        /// <param name="package">
        /// The hosting package.
        /// </param>
        internal RefactoringCodeCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidRefactoringCode)
        {
        }

        #endregion Constructors

        #region BaseCommand Members
        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {

            Enabled = Package.IDE.Solution.IsOpen;//&& SelectedProjectItems.Count() > 0;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            //TODO:实现具体的业务代码
        }

        #endregion BaseCommand Members
    }
}