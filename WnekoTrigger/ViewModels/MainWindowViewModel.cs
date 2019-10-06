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
using WnekoTrigger.Models;

namespace WnekoTrigger.ViewModels
{
    partial class MainWindowViewModel : INotifyPropertyChanged
    {
        private float volumeLevel;
        private Timer timer;
        private MMDevice selectedRecordDevice;
        private MMDevice selectedPlayDevice;
        private MMDevice defaultOutput;
        private MMDevice defaultInput;
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
        private CommandHandler openHelpCommand;
        private CommandHandler startCommand;
        private CommandHandler stopCommand;
        private CommandHandler setMaxVolume;
        private CommandHandler refreshCommand;
        private bool isStarted = false;
        private List<Mode> modeList;
        private Mode selectedMode;
        private SoundTrigger trigger;

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

        internal void SetDefaults()
        {
            int inputNumber = Devices.ToList().FindIndex((s) => s.ID == defaultInput.ID);
            int outputNumber = Devices.ToList().FindIndex((s) => s.ID == defaultOutput.ID);
            main.recordDeviceListBox.SelectedIndex = inputNumber;
            main.playDeviceListBox.SelectedIndex = outputNumber;
            SetMaxVolume.RaiseCanExecuteChanged();
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
                SetMaxVolume.RaiseCanExecuteChanged();
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
        public CommandHandler OpenHelpCommand { get => openHelpCommand; set => openHelpCommand = value; }
        public CommandHandler StartCommand { get => startCommand; set => startCommand = value; }
        public CommandHandler StopCommand { get => stopCommand; set => stopCommand = value; }
        public CommandHandler SetMaxVolume { get => setMaxVolume; set => setMaxVolume = value; }
        public CommandHandler RefreshCommand { get => refreshCommand; set => refreshCommand = value; }
        public List<Mode> ModeList { get => modeList; set => modeList = value; }
        public Mode SelectedMode { get => selectedMode; set => selectedMode = value; }
        public bool IsStarted
        {
            get => isStarted;
            set
            {
                isStarted = value;
                OnPropertyChanged();
            }
        }
        public bool IsNotStarted
        {
            get => !isStarted;
            set
            {
                isStarted = !value;
                OnPropertyChanged();
            }
        }


        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            CommandManager.InvalidateRequerySuggested();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            trigger = new SoundTrigger();
            OpenHelpCommand = new CommandHandler(OpenHelp, (b) => true);
            StartCommand = new CommandHandler(Start, StartEnabled);
            StopCommand = new CommandHandler(Stop, StopEnabled);
            SetMaxVolume = new CommandHandler(MaxVolume, MaxVolumeEnabled);
            RefreshCommand = new CommandHandler(Refresh, (o) => true);
            modeList = Modes.modes;
            selectedMode = modeList[0];
            Refresh(null);
            recorder = new WaveInEvent();
            timer = new Timer();
            timer.Interval = 1000 / 30;
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

        private void Refresh(object o)
        {
            deviceEnumerator = new MMDeviceEnumerator();
            Devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            defaultOutput = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            defaultInput = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            if (main.IsInitialized)
            {
                SetDefaults();
            }
        }
    }
}
