using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WnekoTrigger.Models
{
    public enum ModeTypes
    {
        SingleShot,
        MultipleShot,
        SameInterval,
        MultipleInterval
    }

    public static class Modes
    {
        public static readonly List<Mode> modes = new List<Mode>()
        {
            {new Mode {Type = ModeTypes.SingleShot, Name = "Single shot"} },
            {new Mode {Type = ModeTypes.MultipleShot, Name = "Multiple shot" } },
            {new Mode {Type = ModeTypes.SameInterval, Name = "Same interval" } },
            {new Mode {Type = ModeTypes.MultipleInterval, Name = "Multiple interval"} }
        };
    }

    public class Mode
    {
        public ModeTypes Type { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
