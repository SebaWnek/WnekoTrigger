using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        CancellationTokenSource token = new CancellationTokenSource();

        private void OpenHelp(object o)
        {
            if (helpWindow == null || helpWindow.IsLoaded == false)
            {
                helpWindow = new HelpWindow();
                helpWindow.Show();
            }
        }

        private async void Start(object o)
        {
            IsNotStarted = false;
            StopCommand.RaiseCanExecuteChanged();
            StartCommand.RaiseCanExecuteChanged();
            trigger.InputDevice = selectedRecordDevice;
            trigger.OutputDevide = selectedPlayDevice;
            trigger.SetUpTrigger(selectedMode, selectedRecordDevice, selectedPlayDevice, treshold, minimalInterval, sameInterval, intervalCountEnabled, intervalCount, intervals, duration, delay, token.Token);
            try
            {
                await trigger.StartTrigger();
            }
            catch (OperationCanceledException)
            {
            }
            Stop(null);
            //MessageBox.Show("Dupa");
        }

        private void Stop(object o)
        {
            IsNotStarted = true;
            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
            token.Cancel();
            token.Dispose();
            token = new CancellationTokenSource();
            //MessageBox.Show("Stop dupa");
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

        private void MaxVolume(object o)
        {
            var device = o as MMDevice;
            if (device != null)
            {
                device.AudioEndpointVolume.MasterVolumeLevel = 0;
            }
        }

        private bool MaxVolumeEnabled(object o)
        {
            var device = o as MMDevice;
            if (device == null) return false;
            return true;
        }
    }
}
