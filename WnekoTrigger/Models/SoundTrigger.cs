using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WnekoTrigger.Models
{
    public class SoundTrigger
    {
        MMDevice inputDevice;
        MMDevice outputDevide;

        public SoundTrigger() { }
        public SoundTrigger(MMDevice inputDev, MMDevice outputDev)
        {
            InputDevice = inputDev;
            OutputDevide = outputDev;
        }

        public MMDevice InputDevice { get => inputDevice; set => inputDevice = value; }
        public MMDevice OutputDevide { get => outputDevide; set => outputDevide = value; }
    }
}
