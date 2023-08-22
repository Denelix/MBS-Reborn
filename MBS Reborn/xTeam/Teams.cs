using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS_Reborn.Character;

namespace MBS_Reborn.xTeam
{
    public class Team : Characters
    {
        public List<Characters> characters { get; set; }
        public Characters top { get; set; } = new Characters();
        public Characters jg { get; set; } = new Characters();
        public Characters mid { get; set; } = new Characters();
        public Characters adc { get; set; } = new Characters();
        public Characters sup { get; set; } = new Characters();
        public Characters teamStats { get; set; }
        public Dictionary<Characters, short> levels { get; set; }
        public Dictionary<Characters, double> gold { get; set; }
        public bool checkRoles(Characters Character)
        {
            if (top == Character && top!= null)
            {
                return true;
            }
            if (jg == Character && jg != null)
            {
                return true;
            }
            if (mid == Character && mid != null)
            {
                return true;
            }
            if (adc == Character && adc != null)
            {
                return true;
            }
            if (sup == Character && sup != null)
            {
                return true;
            }
            return false;
        }
    }
}
