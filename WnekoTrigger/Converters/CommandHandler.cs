using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WnekoTrigger.Converters
{
    class CommandHandler : ICommand
    {
        private Action action;
        private Func<bool> canExecute;

        public CommandHandler(Action act, Func<bool> canExec)
        {
            action = act;
            canExecute = canExec;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
