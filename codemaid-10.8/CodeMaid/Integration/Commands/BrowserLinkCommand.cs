using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace Waodng.CodeMaid.Integration.Commands
{
    sealed class BrowserLinkCommand : BaseCommand
    {
        #region Singleton

        public static BrowserLinkCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new BrowserLinkCommand(package);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserLinkCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal BrowserLinkCommand(CodeMaidPackage package)
            : base(package, PackageGuids.guidBrowserReloadCmdSet, PackageIds.EnableReloadCommandId)
        {
        }

        #endregion Constructors

        #region BaseCommand Members

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            //Enabled = Package.IDE.Solution.IsOpen;
            this.Checked = CodeMaidPackage.Options.EnableReload;

            //var button = (OleMenuCommand)sender;
            //button.Checked = VSPackage.Options.EnableReload;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            //var button = (OleMenuCommand)sender;

            CodeMaidPackage.Options.EnableReload = !this.Checked;
            CodeMaidPackage.Options.SaveSettingsToStorage();
        }

        #endregion BaseCommand Members

    }
}
