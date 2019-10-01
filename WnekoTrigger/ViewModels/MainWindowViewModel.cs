using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Windows.Media;
using System.Windows.Input;
using WnekoTrigger.Converters;
using System.Windows;
using WnekoTrigger.Views;

namespace WnekoTrigger.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private float volumeLevel;
        private Timer timer;
        private MMDevice selectedRecordDevice;
        private MMDevice selectedPlayDevice;
        private MMDeviceEnumerator deviceEnumerator;
        private MMDeviceCollection devices;
        public event PropertyChangedEventHandler PropertyChanged;
        private MainWindow main = (MainWindow)App.Current.MainWindow;
        private WaveInEvent recorder;
        private Brush barColor = Brushes.Green;
        private double treshold;
        private double sliderSize;
        private int minimalInterval = 20;
        private int sameInterval = 1000;
        private bool intervalCountEnabled = false;
        private int intervalCount = 5;
        private string intervals = "";
        private int duration = 20;
        private int delay = 0;
        private Window helpWindow;
        private ICommand openHelpCommand;

        public string Intervals { get => intervals; set => intervals = value; }
        public float VolumeLevel
        {
            get => volumeLevel;
            set
            {
                volumeLevel = value;
                OnPropertyChanged();
            }
        }

        public MMDeviceCollection Devices
        {
            get => devices;
            set
            {
                devices = value;
                OnPropertyChanged();
            }
        }

        public MMDevice SelectedRecordDevice
        {
            get => selectedRecordDevice;
            set
            {
                selectedRecordDevice = value;
                OnPropertyChanged();
                sliderSize = main.tresholdSlider.Maximum;
                if (timer.Enabled == false && selectedRecordDevice != null) timer.Start();
                else if (selectedRecordDevice == null && timer.Enabled == true) timer.Stop();

                try
                {
                    recorder.StartRecording();
                }
                catch (InvalidOperationException e)
                {
                    if (!e.Message.Contains("Already recording")) throw;
                }
            }
        }
        public MMDevice SelectedPlayDevice
        {
            get => selectedPlayDevice;
            set
            {
                selectedPlayDevice = value;
                OnPropertyChanged();
            }
        }

        public Brush BarColor
        {
            get => barColor;
            set
            {
                barColor = value;
                OnPropertyChanged();
            }
        }

        public double Treshold
        {
            get => treshold * sliderSize;
            set
            {
                sliderSize = main.tresholdSlider.Maximum;
                treshold = value / sliderSize;
                OnPropertyChanged();
            }
        }

        public int MinimalInterval { get => minimalInterval; set => minimalInterval = value; }
        public int SameInterval { get => sameInterval; set => sameInterval = value; }
        public bool IntervalCountEnabled { get => intervalCountEnabled; set => intervalCountEnabled = value; }
        public int IntervalCount { get => intervalCount; set => intervalCount = value; }
        public int Duration { get => duration; set => duration = value; }
        public int Delay { get => delay; set => delay = value; }
        public ICommand OpenHelpCommand { get => openHelpCommand; set => openHelpCommand = value; }

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            
            OpenHelpCommand = new CommandHandler(OpenHelp, (b) => true);
            deviceEnumerator = new MMDeviceEnumerator();
            Devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            recorder = new WaveInEvent();
            timer = new Timer();
            timer.Interval = 1000/30;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SelectedRecordDevice != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    VolumeLevel = SelectedRecordDevice.AudioMeterInformation.MasterPeakValue;
                    if (volumeLevel > treshold) BarColor = Brushes.Red;
                    else BarColor = Brushes.Green;
                });
            }
        }

        private void OpenHelp(object o)
        {
            MessageBox.Show("Dupa");
            if (helpWindow == null || helpWindow.IsLoaded == false)
            {
                helpWindow = new HelpWindow();
                helpWindow.Show();
            }
        }
    }
}
