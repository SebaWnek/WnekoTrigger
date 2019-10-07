using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WnekoTrigger.Models
{
    public class SoundTrigger
    {
        SignalGenerator signal;
        WaveOutEvent wo;
        private delegate Task Function();
        Function choosenMethod;
        CancellationToken cancellationToken;

        Mode triggerMode;
        MMDevice inputDevice;
        MMDevice outputDevice;
        int minimalInterval;
        int sameInterval;
        bool intervalCountEnabled;
        int intervalCount;
        string intervals;
        int[] intervalsArray;
        int duration;
        int delay;
        double treshold;

        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\wneko\Source\Repos\SebaWnek\WnekoTrigger\WnekoTrigger\Resources\shot.wav");

        float readVolume;

        public SoundTrigger() : this(null, null)
        {
        }
        public SoundTrigger(MMDevice inputDev, MMDevice outputDev)
        {
            InputDevice = inputDev;
            OutputDevide = outputDev;
            signal = new SignalGenerator() { Gain = 1, Frequency = 500, Type = SignalGeneratorType.Pink };
            //for now will always play on default device, going to change this later to play on outputDevice
            wo = new WaveOutEvent();
            wo.Init(signal);
        }

        public MMDevice InputDevice { get => inputDevice; set => inputDevice = value; }
        public MMDevice OutputDevide { get => outputDevice; set => outputDevice = value; }

        private async Task PlayAsync()
        {
            //wo.Play();
            //await Task.Delay(duration);
            //wo.Pause();
            player.Play();

            await Task.Delay(duration);
        }

        public void SetUpTrigger(Mode mode, MMDevice input, MMDevice output, double tresh, int minInter, int sameInter, bool intCountEnabled, int intCount, string inters, int dur, int del, CancellationToken token)
        {
            triggerMode = mode;
            inputDevice = input;
            outputDevice = output;
            treshold = tresh;
            minimalInterval = minInter;
            sameInterval = sameInter;
            intervalCountEnabled = intCountEnabled;
            if (intervalCountEnabled)
            {
                intervalCount = intCount;
            }
            else intervalCount = int.MaxValue;
            intervals = inters;
            duration = dur;
            delay = del;
            cancellationToken = token;
            intervalsArray = ReadIntervals(intervals);

            ChoosMethod();
        }

        private void ChoosMethod()
        {
            switch (triggerMode.Type)
            {
                case ModeTypes.SingleShot:
                    choosenMethod = SingleShot;
                    break;
                case ModeTypes.MultipleShot:
                    choosenMethod = MultiShot;
                    break;
                case ModeTypes.SameInterval:
                    choosenMethod = SameInterval;
                    break;
                case ModeTypes.MultipleInterval:
                    choosenMethod = MultiIntervals;
                    break;
            }
        }

        private int[] ReadIntervals(string intervals)
        {
            List<int> tmp = new List<int>();
            string[] strings = intervals.Split(new char[] { ',', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in strings)
            {
                tmp.Add(int.Parse(s));
            }
            return tmp.ToArray();
        }

        public async Task StartTrigger()
        {
            await choosenMethod();
        }

        private async Task SingleShot()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested == true) return;
                    readVolume = inputDevice.AudioMeterInformation.MasterPeakValue;
                    if (readVolume >= treshold)
                    {
                        Thread.Sleep(delay);
                        PlayAsync();
                        return;
                    }
                }
            });
        }

        private async Task MultiShot()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested == true) return;
                    readVolume = inputDevice.AudioMeterInformation.MasterPeakValue;
                    if (readVolume >= treshold)
                    {
                        Thread.Sleep(delay);
                        PlayAsync();
                        Thread.Sleep(minimalInterval);
                    }
                }
            });
        }

        private async Task SameInterval()
        {
            for (int i = 0; i < intervalCount; i++)
            {
                if (cancellationToken.IsCancellationRequested == true) return;
                PlayAsync();
                await Task.Delay(sameInterval);
            }
        }

        private async Task MultiIntervals()
        {
            await PlayAsync();
            foreach (int delay in intervalsArray)
            {
                if (cancellationToken.IsCancellationRequested == true) return;
                await Task.Delay(delay);
                PlayAsync();
            }
        }
    }
}
