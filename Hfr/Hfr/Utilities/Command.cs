using System;
using System.Windows.Input;

namespace Hfr.Utilities
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