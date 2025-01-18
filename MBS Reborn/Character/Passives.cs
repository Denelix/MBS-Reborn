using MBS_Reborn.xTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS_Reborn.Character
{
    public class Passives
    {

        public void OnhitBenefit(double multiplier)
        {
            //loop items and multiply damage based off their onhit multiplier
        }
    }
    public class Effect
    {
        public string EffectName { get; set; }
        public double Duration { get; set; }
        public double Magnitude { get; set; }

        public Effect(string effectName, double duration, double magnitude)
        {
            EffectName = effectName;
            Duration = duration;
            Magnitude = magnitude;
        }
    }
}
