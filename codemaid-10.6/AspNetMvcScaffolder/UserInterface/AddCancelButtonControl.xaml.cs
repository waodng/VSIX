using System.Windows;
using System.Windows.Controls;

namespace AspNetMvcScaffolder.UserInterface
{
    internal partial class AddCancelButtonControl : UserControl
    {
        public event RoutedEventHandler AddButtonClick;

        public AddCancelButtonControl()
        {
            InitializeComponent();
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddButtonClick != null)
            {
                AddButtonClick(sender, e);
            }
        }
    }
}
