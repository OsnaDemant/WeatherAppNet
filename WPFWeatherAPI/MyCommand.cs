using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFWeatherAPI
{
    public class MyCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public MyCommand(Action execute,Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;

        }

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke();
          
        }
        public void Execute(object parameter)
        {
            execute?.Invoke();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

