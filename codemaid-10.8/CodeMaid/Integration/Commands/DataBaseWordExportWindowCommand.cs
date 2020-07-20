using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Waodng.CodeMaid.UI.Dialogs.DBCHM;

namespace Waodng.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the Spade tool window.
    /// </summary>
    internal sealed class DataBaseWordExportWindowCommand : BaseCommand
    {
        #region Singleton

        public static DataBaseWordExportWindowCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new DataBaseWordExportWindowCommand(package);
            package.SettingsMonitor.Watch(s => s.Feature_DataBaseWordWindow, Instance.Switch);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseWordExportWindowCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal DataBaseWordExportWindowCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidDataBaseWordExportWindow)
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();
            new DBCHMWindow { DataContext = new DBCHMViewModel(Package) }.ShowModal(); 
            //DBCHM.Startup.Show();
        }

        public override void Switch(bool on)
        {
            base.Switch(on);

            if (!on)
            {
                Package.Spade?.Close();
            }
        }

        #endregion BaseCommand Methods

        #region Internal Methods

        /// <summary>
        /// Called when a window change has occurred, potentially to be used by the Spade tool window.
        /// </summary>
        /// <param name="document">The document that got focus, may be null.</param>
        internal void OnWindowChange(Document document)
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                spade.NotifyActiveDocument(document);
            }
        }

        #endregion Internal Methods
    }
}