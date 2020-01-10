using EnvDTE;
using Waodng.CodeMaid.Logic.NestIn;
using System.Collections.Generic;
using System.Linq;

namespace Waodng.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up
    /// code in the selected documents.
    /// </summary>
    internal sealed class NestOutFileCommand : BaseCommand
    {
        #region Singleton

        public static NestOutFileCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new NestOutFileCommand(package);
            //监控配置文件决定是否启用该功能，不然无法显示按钮
            package.SettingsMonitor.Watch(s => s.NestInOutFile, Instance.Switch);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="NestOutFileCommand" /> class.
        /// </summary>
        /// <param name="package">
        /// The hosting package.
        /// </param>
        internal NestOutFileCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidNestOutFile)
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
                new WorkerLogic(Package.IDE, rootSelector).UnNest();
            }
        }

        #endregion BaseCommand Members
    }
}