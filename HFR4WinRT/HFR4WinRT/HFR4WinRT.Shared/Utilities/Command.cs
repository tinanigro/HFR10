using System;
using System.Windows.Input;

namespace HFR4WinRT.Utilities
{
    public abstract class Command : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;
    }
}
