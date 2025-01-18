using System;
using System.Collections.Generic;
using MBS_Reborn.Character;
using MBS_Reborn.BattleSimulator;
using MBS_Reborn.xTeam;
using Microsoft.Office.Interop.Excel;
using Characters = MBS_Reborn.Character.Characters;
using System.Reflection;

namespace MBS_Reborn.BattleSimulator
{
    public class Match
    {
        private Random random = new Random();
        private Team[] teams;
        private double gameTime; // in minutes
        private int currentTime; // in seconds
        private List<TemporaryStats> tempo;
        private List<Characters> tempos;
        // Statistics Tracking
        public List<MatchStats> MatchStatistics { get; set; } = new List<MatchStats>();

        public Match(Team[] teamArray, string elo, List<Stats> statsList, List<TemporaryStats> temp, List<Characters> chars)
        {
            if (teamArray.Length != 2)
                throw new ArgumentException("Match must have exactly two teams.");

            teams = teamArray;
            tempo = temp;
            tempos = chars;
            InitializeMatchTime();
            InitializeTeams();
        }

        // Initialize the game time randomly
        private void InitializeMatchTime()
        {
            // Total game time between 15 and 35 minutes
            gameTime = 15 + (random.NextDouble() * 20);
            currentTime = 0;
        }

        // Initialize teams (e.g., set starting gold)
        private void InitializeTeams()
        {
            foreach (Team team in teams)
            {
                // Initialize each FightCharacter's starting gold
                team.Top.Gold = 500.0;
                team.Jungle.Gold = 500.0;
                team.Mid.Gold = 500.0;
                team.ADC.Gold = 500.0;
                team.Support.Gold = 500.0;
                team.Top.LowBaseHealth = team.Top.BaseHealth;
                team.Jungle.LowBaseHealth = team.Jungle.BaseHealth;
                team.Mid.LowBaseHealth = team.Mid.BaseHealth;
                team.ADC.LowBaseHealth = team.ADC.BaseHealth;
                team.Support.LowBaseHealth = team.Support.BaseHealth;
                team.Top.BaseMovementSpeed = team.Top.BaseMovementSpeed;
                team.Jungle.BaseMovementSpeed = team.Jungle.BaseMovementSpeed;
                team.Mid.BaseMovementSpeed = team.Mid.BaseMovementSpeed;
                team.ADC.BaseMovementSpeed = team.ADC.BaseMovementSpeed;
                team.Support.BaseMovementSpeed = team.Support.BaseMovementSpeed;
                Console.WriteLine(team.Top.Name);
                // Initialize other FightCharacter properties if needed
                // For example, setting starting items, runes, level, etc.
            }
        }

        // Start the match simulation
        public void StartMatch()
        {
            Console.WriteLine("Match Started!");
            Console.WriteLine($"Game Duration: {gameTime:F2} minutes");

            // Laning Phase: Typically first 15 minutes
            while (currentTime < gameTime * 60)
            {
                currentTime += 60; // Increment game time by one minute (60 seconds)

                // Handle events per minute
                HandleLaningPhase(currentTime);
                HandleSkirmishes();
                HandleGoldAccumulation();

                // Optionally: Handle other game events (e.g., objective control)
            }

            // End of Match
            DetermineWinner();
            DisplayMatchResults();
        }

        // Handle laning phase events
        private void HandleLaningPhase(int timeInSeconds)
        {
            // Example: At specific times, trigger laning duels
            // For simplicity, let's assume duels happen randomly per minute
            // Replace with your own logic for triggering duels

            // Random chance to trigger a duel
            if (random.NextDouble() < 0.35) 
            {
                FightSimulator x = new FightSimulator(teams[0].Top, teams[1].Top);
                x.Duel();
            }

            if (random.NextDouble() < 0.25)
            {
                FightSimulator x = new FightSimulator(teams[0].Mid, teams[1].Mid);
                x.Duel();
            }

            if (random.NextDouble() < 0.25)
            {
                FightSimulator x = new FightSimulator(teams[0].ADC, teams[1].ADC);
                x.Duel();
            }

            if (random.NextDouble() < 0.2)
            {
                FightSimulator x = new FightSimulator(teams[0].Jungle, teams[1].Jungle);
                x.Duel();
            }

            if (random.NextDouble() < 0.1)
            {
                FightSimulator x = new FightSimulator(teams[0].Support, teams[1].Support);
                x.Duel();
            }
            if (random.NextDouble() < 0.1)
            {
                FightSimulator x = new FightSimulator(teams[0].ADC, teams[1].Support);
                x.Duel();
            }
            if (random.NextDouble() < 0.1)
            {
                FightSimulator x = new FightSimulator(teams[0].Support, teams[1].ADC);
                x.Duel();
            }
        }

        // Handle duels between FightCharacters

        // Handle skirmishes based on gold lead and game progression
        private void HandleSkirmishes()
        {
            // Determine if a skirmish should occur
            double skirmishChance = CalculateSkirmishChance();
            if (random.NextDouble() < skirmishChance)
            {
                // Determine number of participants (1 to 5)
                int participants = random.Next(1, 6); // Random between 1 and 5

                // Select participants from both teams
                List<FightCharacter> teamA = SelectSkirmishParticipants(teams[0], participants);
                List<FightCharacter> teamB = SelectSkirmishParticipants(teams[1], participants);

                if (teamA.Count > 0 && teamB.Count > 0)
                {
                    // Execute the skirmish
                    Skirmish(teamA, teamB);
                }
            }
        }

        // Calculate the chance of a skirmish occurring
        private double CalculateSkirmishChance()
        {
            // Base chance increases with game time
            double baseChance = gameTime > 25 ? 0.3 :
                                gameTime > 20 ? 0.2 :
                                gameTime > 15 ? 0.1 : 0.05;

            // Increase chance if teams have a significant gold lead
            double goldDifference = Math.Abs(teams[0].Gold - teams[1].Gold);
            if (goldDifference > 1000)
            {
                baseChance += 0.05; // Additional 5% chance
            }

            // Cap the chance at 50%
            return Math.Min(baseChance, 0.5);
        }

        // Select skirmish participants from a team
        private List<FightCharacter> SelectSkirmishParticipants(Team team, int maxParticipants)
        {
            List<FightCharacter> participants = new List<FightCharacter>();

            // Prioritize jungler if there's a gold lead
            FightCharacter jungler = team.Jungle;
            if (groupHasGoldLead(team))
            {
                participants.Add(jungler);
            }

            // Remaining participants
            List<FightCharacter> otherRoles = new List<FightCharacter>
            {
                team.Top,
                team.Mid,
                team.ADC,
                team.Support
            };

            int additional = maxParticipants - participants.Count;
            for (int i = 0; i < additional; i++)
            {
                if (otherRoles.Count == 0)
                    break;

                int index = random.Next(otherRoles.Count);
                participants.Add(otherRoles[index]);
                otherRoles.RemoveAt(index);
            }

            return participants;
        }

        // Check if a team has a gold lead
        private bool groupHasGoldLead(Team team)
        {
            // Compare team's gold to the opposing team
            Team opposingTeam = (teams[0] == team) ? teams[1] : teams[0];
            return team.Gold > opposingTeam.Gold + 500; // Example threshold
        }

        // Execute a skirmish between two groups of FightCharacters
        private void Skirmish(List<FightCharacter> teamA, List<FightCharacter> teamB)
        {
            // Placeholder for skirmish logic
            // For simplicity, determine the winning team based on cumulative scores
        }

        // Calculate cumulative score for a team of FightCharacters
        private double CalculateTeamScore(List<FightCharacter> team)
        {
            double score = 0.0;
            foreach (FightCharacter character in team)
            {
                score += character.InitPickRate(new Stats()); // Assuming InitPickRate calculates a score
            }
            return score;
        }

        // Simulate team victory based on scores
        private FightCharacter SkirmishMethod(List<FightCharacter> teamA, List<FightCharacter> teamB, double teamAScore, double teamBScore)
        {
            double totalScore = teamAScore + teamBScore;
            if (totalScore == 0)
                return null;

            double randValue = random.NextDouble() * totalScore;
            return randValue < teamAScore ? SimulateTeamVictory(teamA) : SimulateTeamVictory(teamB);
        }

        // Simulate team victory by randomly selecting a member from the winning team
        private FightCharacter SimulateTeamVictory(List<FightCharacter> team)
        {
            if (team.Count == 0)
                return null;

            int index = random.Next(team.Count);
            return team[index];
        }

        // Get a random character from a team
        private FightCharacter GetRandomCharacter(List<FightCharacter> team)
        {
            if (team.Count == 0)
                return null;

            int index = random.Next(team.Count);
            return team[index];
        }

        // Handle gold accumulation over time
        private void HandleGoldAccumulation()
        {
            // Simple gold accumulation: Each minute, characters earn gold
            foreach (Team team in teams)
            {
                foreach (FightCharacter character in new List<FightCharacter> { team.Top, team.Jungle, team.Mid, team.ADC, team.Support })
                {
                    // Example gold gain logic: each character gains gold based on some criteria
                    // This is a placeholder and should be replaced with actual game mechanics like CS, objectives, etc.
                    double goldGain = 50 + (character.Level * random.NextInt64(5, 12)); // Example calculation
                    character.Gold += goldGain;
                    team.Gold += goldGain;
                    character.Mana += character.ManaRegen * 100;
                    character.Health += character.HealthRegen * 100;
                    double expGain = 150 + (character.Level * random.NextInt64(5, 12)); // Example calculation
                    FightSimulator simulator = new FightSimulator(character, character);
                    simulator.AddExp(character, expGain);
                    character.ability1Cooldown = Math.Max(0, character.ability1Cooldown - 60);
                    character.ability2Cooldown = Math.Max(0, character.ability2Cooldown - 60);
                    character.ability3Cooldown = Math.Max(0, character.ability3Cooldown - 60);
                    character.ability4Cooldown = Math.Max(0, character.ability4Cooldown - 60);
                }
            }

            Console.WriteLine("Gold accumulated for this minute.");
        }

        // Determine the winning team based on total kills
        private void DetermineWinner()
        {
            // Find the team with the most kills
            int topKills = -1;
            Team x = null;

            foreach (Team team in teams)
            {
                if (team.Kills > topKills)
                {
                    x = team;
                    topKills = team.Kills;
                }
                var players = new List<Characters>
                {
                    x.Support, x.Mid,x.Top,x.Jungle, x.ADC,
                };
                // "Im just one guy of many brains so as long as it works it works. idc anymore."
                //foreach (TemporaryStats player in tempo)
                //{
                //    var xa = new TemporaryStats();
                //    if (team.Support.Name == player.name)
                //    {
                //        xa = tempo.Find(tempos => tempos.name == player.name);
                //        xa.Kills += team.Support.Kills;
                //        xa.Deaths += team.Support.Deaths;
                //        xa.Assists += team.Support.Assists;
                //    }
                //    if (team.Mid.Name == player.name)
                //    {
                //        xa = tempo.Find(tempos => tempos.name == player.name);
                //        xa.Kills += team.Mid.Kills;
                //        xa.Deaths += team.Mid.Deaths;
                //        xa.Assists += team.Mid.Assists;
                //    }
                //    if (team.Top.Name == player.name)
                //    {
                //        xa = tempo.Find(tempos => tempos.name == player.name);
                //        xa.Kills += team.Top.Kills;
                //        xa.Deaths += team.Top.Deaths;
                //        xa.Assists += team.Top.Assists;
                //    }
                //    if (team.Jungle.Name == player.name)
                //    {
                //        xa = tempo.Find(tempos => tempos.name == player.name);
                //        xa.Kills += team.Jungle.Kills;
                //        xa.Deaths += team.Jungle.Deaths;
                //        xa.Assists += team.Jungle.Assists;
                //    }
                //    if (team.ADC.Name == player.name)
                //    {
                //        xa = tempo.Find(tempos => tempos.name == player.name);
                //        xa.Kills += team.ADC.Kills;
                //        xa.Deaths += team.ADC.Deaths;
                //        xa.Assists += team.ADC.Assists;
                //    }
                //}

                // ???????? or just do this???? No brain does allat
                var teamPlayers = new List<FightCharacter> { team.Support, team.Mid, team.Top, team.Jungle, team.ADC };

                foreach (var teamPlayer in teamPlayers)
                {
                    var matchingPlayer = tempo.Find(player => player.name == teamPlayer.Name);
                    if (matchingPlayer != null)
                    {
                        matchingPlayer.Kills += teamPlayer.Kills;
                        matchingPlayer.Deaths += teamPlayer.Deaths;
                        matchingPlayer.Assists += teamPlayer.Assists;
                    }
                }

            }

            var winners = new List<Characters>
            {
                x.Support, x.Mid,x.Top,x.Jungle, x.ADC,
            };
            // Find stats related to the winning team
            foreach (TemporaryStats winner in tempo)
            {
                // Check if the winner matches each role and increment the respective win count
                if (x.Support.Name == winner.name)
                {
                    winner.winSupport++;
                    tempos.Find(tempos => tempos.Name == winner.name).wins++;
                }
                if (x.Mid.Name == winner.name)
                { 
                    winner.winMid++;
                    tempos.Find(tempos => tempos.Name == winner.name).wins++;
                }
                if (x.Top.Name == winner.name)
                { 
                    winner.winTop++;
                    tempos.Find(tempos => tempos.Name == winner.name).wins++;
                }
                if (x.Jungle.Name == winner.name)
                { 
                    winner.winJungle++;
                    tempos.Find(tempos => tempos.Name == winner.name).wins++;
                }
                if (x.ADC.Name == winner.name)
                { 
                    winner.winADC++;
                    tempos.Find(tempos => tempos.Name == winner.name).wins++;
                }
            }

            // Iterate through the winner stats and perform actions (if necessary)
        }

        // Display the match results
        private void DisplayMatchResults()
        {
            Console.WriteLine("Match Ended!");
            foreach (Team team in teams)
            {
                Console.WriteLine(team);
            }

            Team winningTeam = DetermineWinningTeam();
            Console.WriteLine($"Winning Team: {(winningTeam == teams[0] ? "A" : "B")}");
        }

        // Determine the winning team based on skirmish wins
        private Team DetermineWinningTeam()
        {
            Team team1 = teams[0];
            Team team2 = teams[1];

            if (team1.SkirmishWins > team2.SkirmishWins)
                return team1;
            else if (team2.SkirmishWins > team1.SkirmishWins)
                return team2;
            else
                return team1; // Default to Team A in case of tie
        }

        // Determine which team a FightCharacter belongs to
        private Team DetermineTeam(FightCharacter character)
        {
            foreach (Team team in teams)
            {
                if (team.ContainsCharacter(character))
                    return team;
            }
            return null;
        }

        // Placeholder for Duel method
        private FightCharacter DuelMethod(FightCharacter attacker, FightCharacter defender)
        {
            // Implement actual duel logic here
            // For now, randomly decide the winner based on some criteria

            double attackerScore = attacker.InitPickRate(new Stats());
            double defenderScore = defender.InitPickRate(new Stats());

            double totalScore = attackerScore + defenderScore;
            double randValue = random.NextDouble() * totalScore;

            return randValue < attackerScore ? attacker : defender;
        }
    }

    // Class to track individual match statistics
    public class MatchStats
    {
        public FightCharacter Character { get; set; }
        public double KDA { get; set; }
        public double Gold { get; set; }
        public int SkirmishWins { get; set; }

        // Add other relevant statistics as needed
    }
}