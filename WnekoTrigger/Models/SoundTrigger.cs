using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WnekoTrigger.Models
{
    public class SoundTrigger
    {
        SignalGenerator signal;
        WaveOutEvent wo;

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

        public SoundTrigger() : this(null, null)
        {
        }
        public SoundTrigger(MMDevice inputDev, MMDevice outputDev)
        {
            InputDevice = inputDev;
            OutputDevide = outputDev;
            signal = new SignalGenerator() { Gain = 1, Frequency = 500, Type = SignalGeneratorType.Sin };
            //for now will always play on default device, going to change this later to play on outputDevice
            wo = new WaveOutEvent();
            wo.Init(signal);
        }

        public MMDevice InputDevice { get => inputDevice; set => inputDevice = value; }
        public MMDevice OutputDevide { get => outputDevice; set => outputDevice = value; }

        private async Task PlayAsync()
        {
            wo.Play();
            await Task.Delay(duration);
            wo.Pause();
        }

        public void SetUpTrigger(Mode mode, MMDevice input, MMDevice output, int minInter, int sameInter, bool intCountEnabled, int intCount, string inters, int dur, int del)
        {
            triggerMode = mode;
            inputDevice = input;
            outputDevice = output;
            minimalInterval = minInter;
            sameInterval = sameInter;
            intervalCountEnabled = intCountEnabled;
            intervalCount = intCount;
            intervals = inters;
            duration = dur;
            delay = del;
            intervalsArray = ReadIntervals(intervals);
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
    }
}
