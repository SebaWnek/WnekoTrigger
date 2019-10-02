using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WnekoTrigger.Converters
{
    class CommandHandler : ICommand
    {
        private Action<object> action;
        private Func<object, bool> canExecute;

        public CommandHandler(Action<object> act, Func<object, bool> canExec)
        {
            action = act;
            canExecute = canExec;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
