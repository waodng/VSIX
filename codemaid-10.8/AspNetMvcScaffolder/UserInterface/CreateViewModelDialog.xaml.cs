using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace AspNetMvcScaffolder.UserInterface
{
    internal partial class CreateViewModelDialog : ValidatingDialogWindow
    {
        public CreateViewModelDialog()
        {
            InitializeComponent();
            TestData();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This is called in xaml.")]
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            TryClose();
        }

        public void TestData()
        {
            InitializeComponent();
        }

        private void HeadCheck(object sender, RoutedEventArgs e, bool isChecked)
        {
            dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            foreach (ViewModelProperty viewModel in dataGrid.Items)
            {
                viewModel.Checked = isChecked;
            }
            dataGrid.Items.Refresh();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            HeadCheck(sender, e, true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HeadCheck(sender, e, false);
        }
    }
}
