using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using MBS_Reborn.Character;

namespace MBS_Reborn.xTeam
{
    public class Team
    {
        // Role-specific FightCharacters
        public FightCharacter Top { get; set; }
        public FightCharacter Jungle { get; set; }
        public FightCharacter Mid { get; set; }
        public FightCharacter ADC { get; set; }
        public FightCharacter Support { get; set; }

        // Team Statistics
        public int Kills { get; set; } = 0;
        public double Gold { get; set; } = 500.0; // Starting gold
        public int SkirmishWins { get; set; } = 0;

        // Optional: Additional team-wide statistics can be added here

        public Team()
        {
            // Initialize FightCharacters for each role
            Top = new FightCharacter();
            Jungle = new FightCharacter();
            Mid = new FightCharacter();
            ADC = new FightCharacter();
            Support = new FightCharacter();
        }

        // Optional: Overloaded constructor to initialize with specific FightCharacters
        public Team(FightCharacter top, FightCharacter jungle, FightCharacter mid, FightCharacter adc, FightCharacter support)
        {
            Top = top;
            Jungle = jungle;
            Mid = mid;
            ADC = adc;
            Support = support;
        }

        // Check if a FightCharacter belongs to this team
        public bool ContainsCharacter(FightCharacter character)
        {
            return Top == character || Jungle == character || Mid == character || ADC == character || Support == character;
        }

        // Add a kill to the team
        public void AddKill()
        {
            Kills++;
        }
        public bool checkRoles(Characters Character)
        {
            if (Top == Character && Top != null)
            {
                return true;
            }
            if (Jungle == Character && Jungle != null)
            {
                return true;
            }
            if (Mid == Character && Mid != null)
            {
                return true;
            }
            if (ADC == Character && ADC != null)
            {
                return true;
            }
            if (Support == Character && Support != null)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Top: {Top.Name} | KDA: {Top.Kills}/{Top.Deaths}/{Top.Assists} | Level: {Top.Level}\n" +
                   $"Jungle: {Jungle.Name} | KDA: {Jungle.Kills}/{Jungle.Deaths}/{Jungle.Assists} | Level: {Jungle.Level}\n" +
                   $"Mid: {Mid.Name} | KDA: {Mid.Kills}/{Mid.Deaths}/{Mid.Assists} | Level: {Mid.Level}\n" +
                   $"ADC: {ADC.Name} | KDA: {ADC.Kills}/{ADC.Deaths}/{ADC.Assists} | Level: {ADC.Level}\n" +
                   $"Support: {Support.Name} | KDA: {Support.Kills}/{Support.Deaths}/{Support.Assists} | Level: {Support.Level}";
        }
    }
}