using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Dialogs.CleanupProgress;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in all documents.
    /// </summary>
    internal sealed class CleanupAllCodeCommand : BaseCommand
    {
        #region Singleton

        public static CleanupAllCodeCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new CleanupAllCodeCommand(package);
            package.SettingsMonitor.Watch(s => s.Feature_CleanupAllCode, Instance.Switch);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupAllCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupAllCodeCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidCleanupAllCode)
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
        }

        #endregion Constructors

        #region BaseCommand Members

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.Solution.IsOpen;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            if (!CodeCleanupAvailabilityLogic.IsCleanupEnvironmentAvailable())
            {
                MessageBox.Show(Resources.CleanupCannotRunWhileDebugging,
                                Resources.CodeMaidCleanupAllCode,
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (MessageBox.Show(Resources.AreYouReadyForCodeMaidToCleanEverythingInTheSolution,
                                     Resources.CodeMaidConfirmationForCleanupAllCode,
                                     MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
                         == MessageBoxResult.Yes)
            {
                using (new ActiveDocumentRestorer(Package))
                {
                    var viewModel = new CleanupProgressViewModel(Package, AllProjectItems);
                    var window = new CleanupProgressWindow { DataContext = viewModel };

                    window.ShowModal();
                }
            }
        }

        #endregion BaseCommand Members

        #region Private Properties

        /// <summary>
        /// Gets the list of all project items.
        /// </summary>
        private IEnumerable<ProjectItem> AllProjectItems
        {
            get { return SolutionHelper.GetAllItemsInSolution<ProjectItem>(Package.IDE.Solution).Where(x => CodeCleanupAvailabilityLogic.CanCleanupProjectItem(x)); }
        }

        /// <summary>
        /// Gets or sets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; set; }

        #endregion Private Properties
    }
}