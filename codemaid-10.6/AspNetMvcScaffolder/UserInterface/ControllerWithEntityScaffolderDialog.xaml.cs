using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace AspNetMvcScaffolder.UserInterface
{
    internal partial class ControllerWithEntityScaffolderDialog : ValidatingDialogWindow
    {
        public ControllerWithEntityScaffolderDialog()
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
