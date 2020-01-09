using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AspNetMvcScaffolder.UserInterface
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Using a ref parameter here is intentional")]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Using a default value is required for CallerMemberName")]
        protected bool OnPropertyChanged<T>(ref T propertyRef, T value, [CallerMemberName]string propertyName = null)
        {
            if (!Object.Equals(propertyRef, value))
            {
                propertyRef = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
