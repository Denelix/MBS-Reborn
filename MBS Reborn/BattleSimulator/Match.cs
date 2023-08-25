using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS_Reborn.xTeam;
using MBS_Reborn.Character;
using MBS_Reborn.Debugger;

namespace MBS_Reborn.BattleSimulator
{
    public class Match
    {
        public Random random = new Random();
        public Team[] teams;
        public Match(Team[] teamss, string elo, List<Stats> stats)
        {
            teams = teamss; 
            var timeChanger = 1;
            double gameTime = 15 + (random.NextDouble() * 20);
            while (timeChanger < 10)
            {
                timeChanger = random.Next(1, 35);
                int value = random.Next(-5, 16);
                double moreRandom = random.NextDouble() * 7; // This allows games to have a lower chance of being under 15 minutes.
                if (gameTime < 3)
                {
                    gameTime = 3; //Earliest a game can end due to FF.
                }
            }
        }
/*
        int numOfFights = random.Next(1, 3);
            for (int i=0;  numOfFights!=i; i++)
            {

            }*/
        //LATER PUT THE FUCKING CHARACTERS IN AN ARRAY smh... it saves so much time.
        public void StartMatch()
        {
            bool a = false;
            Characters firstTop=null;
            Characters firstJg = null;
            Characters firstMid = null;
            Characters firstADC = null;
            Characters firstSup = null;
            List<Characters> WINNERS = new List<Characters>();

            foreach (Team team in teams)
            {
                if (a == false)
                {
                    firstTop = team.top;
                    firstJg = team.jg;
                    firstMid = team.mid;
                    firstADC = team.adc;
                    firstSup = team.sup;
                    a = true;
                }
                else
                {
                    WINNERS.Add(SoloFight(team.top, firstTop));
                    WINNERS.Add(SoloFight(team.top, firstJg));
                    WINNERS.Add(SoloFight(team.top, firstMid));
                    WINNERS.Add(SoloFight(team.top, firstADC));
                    WINNERS.Add(SoloFight(team.top, firstSup));
                    WINNERS.Add(SoloFight(team.jg, firstTop));
                    WINNERS.Add(SoloFight(team.jg, firstJg));
                    WINNERS.Add(SoloFight(team.jg, firstMid));
                    WINNERS.Add(SoloFight(team.jg, firstADC));
                    WINNERS.Add(SoloFight(team.jg, firstSup)); 
                    WINNERS.Add(SoloFight(team.mid, firstTop));
                    WINNERS.Add(SoloFight(team.mid, firstJg));
                    WINNERS.Add(SoloFight(team.mid, firstMid));
                    WINNERS.Add(SoloFight(team.mid, firstADC));
                    WINNERS.Add(SoloFight(team.mid, firstSup));
                    WINNERS.Add(SoloFight(team.adc, firstTop));
                    WINNERS.Add(SoloFight(team.adc, firstJg));
                    WINNERS.Add(SoloFight(team.adc, firstMid));
                    WINNERS.Add(SoloFight(team.adc, firstADC));
                    WINNERS.Add(SoloFight(team.adc, firstSup));
                    WINNERS.Add(SoloFight(team.sup, firstTop));
                    WINNERS.Add(SoloFight(team.sup, firstJg));
                    WINNERS.Add(SoloFight(team.sup, firstMid));
                    WINNERS.Add(SoloFight(team.sup, firstADC));
                    WINNERS.Add(SoloFight(team.sup, firstSup));
                    a = false;
                }
            }
            foreach (Team team in teams)
            {
                foreach (Characters winner in WINNERS)
                {
                    if (team.top == winner || team.jg == winner || team.mid == winner || team.adc == winner || team.sup == winner)
                    {
                        team.kills++;
                    }
                }
            }
            if (teams[0].kills < teams[1].kills)
            {
                teams[0].top.wins++;
                teams[0].jg.wins++;
                teams[0].mid.wins++;
                teams[0].adc.wins++;
                teams[0].sup.wins++;
            }
            else
            {
                teams[1].top.wins++;
                teams[1].jg.wins++;
                teams[1].mid.wins++;
                teams[1].adc.wins++;
                teams[1].sup.wins++;
            }

                //First Top vs Top
                //Jungle cleartime gank.
                //Jungle with chance of ganking all lanes. evasion vs unevasive Chances.
                //Jungle vs jungle if similar clear time. 
                //Mid vs Mid
                //Mid vs All lanes
                //ADC + SUP vs ADC + SUP
                //SUP Mid vs Mid
                //use randomizer 1-10 for how many times they fight or gank or something and who to gank
                //Teamfights from kills and level leads.
                //Objective and split pushing last.
            }
        //In theses it will be attacker vs victim. ex team1.mid vs team2.jg
        Characters SoloFight(Characters a, Characters v)
        {
            Tuple<Characters, double>[] scores =
                {
                new Tuple<Characters, double>(a, 0),
                new Tuple<Characters, double>(v, 0)
            };
            double total = 0;

            foreach (Tuple<Characters, double> tuple in scores)
            {
                Characters character = tuple.Item1;
                double score = tuple.Item2;
                score += character.baseMana;
                score += character.scalingMana;
                score += character.baseHealth;
                score += character.healthRegen;
                score += character.scalingHealth;
                score += character.baseAttackDamage;
                score += character.baseAttackRange;
                score += character.baseDamage;
                score += character.scalingDamage;
                score += character.scalingAttackDamage;
                score += character.attackSpeed;
                score += character.attackSpeedScaling;
                score += character.attackSpeedMulti;
                score += character.damageCooldown;
                score += character.damageRange;
                score += character.physicalResistance;
                score += character.physicalResistanceScaling;
                score += character.specialResistance;
                score += character.specialResistanceScaling;
                score += character.movementSpeed;
                score += character.abilityPower;
                score += character.cooldownReduction;
                score += character.criticalStrikeChance;
                score += character.criticalStrikeDamage;
                score += character.lifeSteal;
                score += character.spellVamp;
                score += character.selfShield;
                score += character.allyShield;
                score += character.allyHeal;
                score += character.allyDamageBoost;
                score += character.evasion;
                score += character.unevadable;
                total += score;
            }

            double decide = random.NextDouble() * total;
            if (decide > scores[0].Item2)
            {
                return a;
            }
            return v;
        }

        void BotFight(Characters a, Characters v)
        {

        }
        void JungleGank(Characters a, Characters b, Characters v)
        {

        }
        void TeamFight()
        {
            //chose 3-5 from each team. Ace chances will be taken for who will defeat all (penta): ) 
        }
        void initialize()
        {
            foreach (Team team in teams)
            {
                team.gold = new Tuple<Characters, double>(team.top, 500.0); 
                team.gold = new Tuple<Characters, double>(team.jg, 500.0); 
                team.gold = new Tuple<Characters, double>(team.mid, 500.0);
                team.gold = new Tuple<Characters, double>(team.adc, 500.0); 
                team.gold = new Tuple<Characters, double>(team.sup, 500.0);
            }
        }
        //----------Get elo 
        //----------Randomizer for how long game goes on. If randomized time of game teamfighting and stuff too.
    }
}
