using System;
using System.Windows.Input;

namespace AspNetMvcScaffolder.UserInterface
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute): this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            ExecuteDelegate = execute;
            CanExecuteDelegate = canExecute;
        }

        private Action<object> ExecuteDelegate
        {
            get;
            set;
        }

        private Predicate<object> CanExecuteDelegate
        {
            get;
            set;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate == null)
            {
                return true;
            }
            else
            {
                return CanExecuteDelegate(parameter);
            }
        }

        public void Execute(object parameter)
        {
            ExecuteDelegate(parameter);
        }

        public void SuggestRequery()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
