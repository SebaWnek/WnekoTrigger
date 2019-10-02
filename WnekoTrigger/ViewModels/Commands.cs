using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
            IsStarted = true;
            trigger.InputDevice = selectedRecordDevice;
            trigger.OutputDevide = selectedPlayDevice;
            MessageBox.Show("Dupa");
        }

        private void Stop(object o)
        {
            IsStarted = false;
        }

        private bool StopEnabled(object o)
        {
            return IsStarted;
        }
        
        private bool StartEnabled(object o)
        {
            return !IsStarted;
        }
    }
}
