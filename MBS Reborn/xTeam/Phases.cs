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
        public static List<Characters> Bans(List<Characters> characters, List<TemporaryStats> tempStats, string Elo)
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
                        if (bannedCharacters.Where(c => c.Name == temp.name).ToList().Count < 2)//Lists are awesome.
                        {
                            bannedCharacters.Add(characters.Find(c => c.Name == temp.name));
                            if (bannedCharacters.Where(c => c.Name == temp.name).ToList().Count == 1) { temp.bans++; }
                            break;
                        }
                    }
                }
            }
            return bannedCharacters;
        }
        public static Team[] Picks(List<Characters> characters, List<TemporaryStats> tempStats, string Elo, List<Characters> bans)
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
                            && bans.Where(c => c.Name == temp.name).ToList().Count == 0
                            && picked.Where(c => c.Name == temp.name).ToList().Count == 0)
                        {
                            //looks if character is not already banned or picked :3
                            Characters selectedCharacter = characters.Find(c => c.Name == temp.name);
                            if (validate(choice, teams, selectedCharacter, temp))
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
        private static bool validate(int choice, Team[] teams, Characters selectedCharacter, TemporaryStats temp)
        {
            if (!teams[1].checkRoles(selectedCharacter) && !teams[0].checkRoles(selectedCharacter))
            {
                FightCharacter fightCharacter = new FightCharacter(selectedCharacter);
                var validated = false;
                if (teams[choice % 2].Top.Name == "empty" && //Makes sure character is qualified or off meta. 
                    (selectedCharacter.canTop||offmeta()))
                {
                    validated= true;
                    teams[choice % 2].Top = fightCharacter;
                    temp.pickTop++;
                }
                else if (teams[choice % 2].Jungle.Name == "empty" && //Makes sure character is qualified or off meta. 
                    (selectedCharacter.canJg || offmeta()))
                {
                    validated = true;
                    teams[choice % 2].Jungle = fightCharacter;
                    temp.pickJungle++;
                }
                else if (teams[choice % 2].Mid.Name == "empty" && //Makes sure character is qualified or off meta. 
                    (selectedCharacter.canMid || offmeta()))
                {
                    validated = true;
                    teams[choice % 2].Mid = fightCharacter;
                    temp.pickMid++;
                }
                else if (teams[choice % 2].ADC.Name == "empty" && //Makes sure character is qualified or off meta. 
                    (selectedCharacter.canAdc || offmeta()))
                {
                    validated = true;
                    teams[choice % 2].ADC = fightCharacter;
                    temp.pickADC++;
                }
                else if (teams[choice % 2].Support.Name == "empty" && //Makes sure character is qualified or off meta. 
                    (selectedCharacter.canSup || offmeta()))
                {
                    validated = true;
                    teams[choice % 2].Support = fightCharacter;
                    temp.pickSupport++;
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
            if (offmeta < .01) 
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