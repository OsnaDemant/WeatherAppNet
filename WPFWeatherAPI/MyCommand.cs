using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFWeatherAPI
{
    public class MyCommand : ICommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;

        public MyCommand(Func<Task> execute,Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;

        }

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke();
          
        }
        public async void Execute(object parameter)
        {
            try
            {
                if(execute is not null)
                    await execute();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
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

