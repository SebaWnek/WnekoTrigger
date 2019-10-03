using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WnekoTrigger.Converters;
using WnekoTrigger.Views;

namespace WnekoTrigger.ViewModels
{
    partial class MainWindowViewModel
    {
        private void OpenHelp(object o)
        {
            if (helpWindow == null || helpWindow.IsLoaded == false)
            {
                helpWindow = new HelpWindow();
                helpWindow.Show();
            }
        }

        private void Start(object o)
        {
            IsNotStarted = false;
            StopCommand.RaiseCanExecuteChanged();
            StartCommand.RaiseCanExecuteChanged();
            trigger.InputDevice = selectedRecordDevice;
            trigger.OutputDevide = selectedPlayDevice;
            MessageBox.Show("Dupa");
        }

        private void Stop(object o)
        {
            IsNotStarted = true;
            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
            MessageBox.Show("Stop dupa");
        }

        private bool StopEnabled(object o)
        {
            return IsStarted;
        }
        
        private bool StartEnabled(object o)
        {
            bool error = Validation.GetHasError(main.intervalsBox);
            return !IsStarted && !error;
        }
    }
}
