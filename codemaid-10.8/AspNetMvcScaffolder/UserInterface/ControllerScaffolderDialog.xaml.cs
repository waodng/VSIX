using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace AspNetMvcScaffolder.UserInterface
{
    internal partial class ControllerScaffolderDialog : ValidatingDialogWindow
    {
        public ControllerScaffolderDialog()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This is called in xaml.")]
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            TryClose();
        }
    }
}
