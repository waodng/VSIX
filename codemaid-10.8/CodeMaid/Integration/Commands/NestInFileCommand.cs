using EnvDTE;
using Waodng.CodeMaid.Helpers;
using Waodng.CodeMaid.Logic.NestIn;
using Waodng.CodeMaid.Logic.Cleaning;
using System.Collections.Generic;
using System.Linq;

namespace Waodng.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up
    /// code in the selected documents.
    /// </summary>
    internal sealed class NestInFileCommand : BaseCommand
    {
        #region Singleton

        public static NestInFileCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new NestInFileCommand(package);
            //监控配置文件决定是否启用该功能
            package.SettingsMonitor.Watch(s => s.NestInOutFile, Instance.Switch);
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
        internal NestInFileCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidNestInFile)
        {
        }

        #endregion Constructors

        #region BaseCommand Members

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            using (RootSelector rootSelector = new RootSelector())
            {
                new WorkerLogic(Package.IDE, rootSelector).Nest();
            }
        }

        #endregion BaseCommand Members
    }
}