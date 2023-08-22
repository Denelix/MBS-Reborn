using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MBS_Reborn.Character;
using MBS_Reborn.Debugger;

namespace MBS_Reborn.xTeam
{
    internal class Phases
    {
        public List<Characters> Bans(List<Characters> characters, List<TemporaryStats> tempStats, string Elo)
        {
            List<Characters> bannedCharacters = new List<Characters>();
            Random random = new Random();
            double banSelect = random.NextDouble() * Main.banScores;
            while (bannedCharacters.Count < 4)
            {
                banSelect = random.NextDouble() * Main.banScores;
                foreach (TemporaryStats temp in tempStats)
                {
                    if (temp.banStart <= banSelect && banSelect <= temp.banEnd)
                    {
                        if (bannedCharacters.Where(c => c.name == temp.name).ToList().Count < 2)//Lists are awesome.
                        {
                            bannedCharacters.Add(characters.Find(c => c.name == temp.name));
                            if (bannedCharacters.Where(c => c.name == temp.name).ToList().Count == 1) { temp.bans++; }
                            break;
                        }
                    }
                }
            }
            return bannedCharacters;
        }
        public Team[] Picks(List<Characters> characters, List<TemporaryStats> tempStats, string Elo, List<Characters> bans)
        {
            int choice = 0;
            Team[] teams = { new Team(), new Team() };
            List<Characters> picked = new List<Characters>();
            Random random = new Random();
        pickstart:
            while (choice != 10)
            {
                double pickSelect = random.NextDouble() * Main.pickScores;
                foreach (TemporaryStats temp in tempStats)
                {
                    try
                    {
                        if (temp.pickStart <= pickSelect && pickSelect <= temp.pickEnd
                            && bans.Where(c => c.name == temp.name).ToList().Count == 0
                            && picked.Where(c => c.name == temp.name).ToList().Count == 0)
                        {
                            //looks if character is not already banned or picked :3
                            Characters selectedCharacter = characters.Find(c => c.name == temp.name);
                            if (validate(choice, teams, selectedCharacter))
                            {
                                picked.Add(selectedCharacter);
                                temp.picks++;
                                choice++;
                            }
                            if (choice != 10) { goto pickstart; }
                            else { return teams; } //end sooner if equals 10
                        }
                    }
                    catch { goto pickstart; }
                }
            }
            return teams;
        }
        private static bool validate(int choice, Team[] teams, Characters selectedCharacter)
        {
            if (!teams[1].checkRoles(selectedCharacter) && !teams[0].checkRoles(selectedCharacter))
            {
                if (teams[choice % 2].top.name == "empty" && //Makes sure character is qualified or off meta. 
                    (false||offmeta()))
                {
                    teams[choice % 2].top = selectedCharacter;
                }
                if (teams[choice % 2].jg.name == "empty")
                {
                    teams[choice % 2].jg = selectedCharacter;
                }
                if (teams[choice % 2].mid.name == "empty")
                {
                    teams[choice % 2].mid = selectedCharacter;
                }
                if (teams[choice % 2].adc.name == "empty")
                {
                    teams[choice % 2].adc = selectedCharacter;
                }
                if (teams[choice % 2].sup.name == "empty")
                {
                    teams[choice % 2].sup = selectedCharacter;
                }
                else { return false; }
                return true;
            }
            return false;
        }

        private static bool offmeta()
        {
            Random random = new Random();
            double offmeta = random.NextDouble();
            if (offmeta < .05) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
//in battle phase use time and score vs eachother to determine who won lane and minutes to scale. RNG on levels higher and RNG on more gold.