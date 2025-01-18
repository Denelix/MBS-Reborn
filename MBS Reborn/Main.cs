using MBS_Reborn.Character;
using static MBS_Reborn.Character.Characters;
using Characters = MBS_Reborn.Character.Characters;
using MBS_Reborn.xTeam;
using MBS_Reborn.Debugger;
using MBS_Reborn.BattleSimulator;
using System.Collections.Generic;

namespace MBS_Reborn
{
    public partial class Main : Form
    {
        public string itemFile = @"E:\itemList.xml";
        public string itemStats = @"E:\itemList.xml";
        public string StatsFile = @"E:\CharacterStats.json";
        public string characterAttributes = @"E:\CharacterAttributes.json";
        public string characterStats = @"E:\CharacterStats.xlsx";
        double day = 0;
        public static int gameTime;
        public static int a;
        public static int Kills;
        public static int Deaths;
        public static int Assists;
        public static double pickScores = 0;
        public static double banScores = 0;
        public static String[] names = new string[999];
        public static double TypeDmg = 1; //1 = SingleType / 1.33=Mixed / 2=TypeDmg 
        public double selected;
        public object locker = new object();
        //public static int threads = Convert.ToInt32(MathF.Round(Environment.ProcessorCount / 2));
        public static int threads = 16;
        public Main()
        {
            InitializeComponent();
            Debug.Log("Threads avaliable to use: " + threads);
        }
        //AFTER EVERY ITERATION USE THE MOST RECENT MATCH THE CHARACTERS WENT IN. KEEP REPLACING THEIR MOST RECENT GAME AND ADDING IT TO THE CHARACTER LIST
        //If champions get picked always simulate matches on NEWLY IMPORTED CHARACTERS!
        //Never include scaling on a champion when checking picks and bans please. Those will be decided automatically.
        //Never save import character stats only export.
        private void button1_Click(object sender, EventArgs e)
        {
            ///Calculate Game Time High chance 21-35 and low chance 10-21 and repeated chance of 35+
            ///Load All Character stats 
            ///Create CLone of character stats 
            List<Characters> charactersOG = FromJson(characterAttributes);
            List<Characters> characters = new List<Characters>(charactersOG);
            List<TemporaryStats> temp = new List<TemporaryStats>();
            List<Stats> stats = new List<Stats>();
            List<Tuple<Characters, double>> topScores = new List<Tuple<Characters, double>>();
            List<Tuple<Characters, double>> jgScores = new List<Tuple<Characters, double>>();
            List<Tuple<Characters, double>> midScores = new List<Tuple<Characters, double>>();
            List<Tuple<Characters, double>> adcScores = new List<Tuple<Characters, double>>();
            List<Tuple<Characters, double>> supScores = new List<Tuple<Characters, double>>();

            foreach (Characters character in characters)
            {
                TemporaryStats pickbanStats = new TemporaryStats();
                Stats characterStats = new Stats();

                pickbanStats.name = character.Name;
                temp.Add(pickbanStats);
                characterStats.name = character.Name;
                stats.Add(characterStats);

                Tuple<Characters,double> midScore = Mid(character);
                Tuple<Characters, double> topScore = Top(character);
                Tuple<Characters, double> adcScore = ADC(character);
                Tuple<Characters, double> jgScore = Jg(character);
                Tuple<Characters, double> supScore = Sup(character);

                midScores.Add(midScore);
                topScores.Add(topScore);
                adcScores.Add(adcScore);
                jgScores.Add(jgScore);
                supScores.Add(supScore);
            }
            var topPercent = topScores.Count/2;
            topScores = topScores.OrderByDescending(t => t.Item2).Take(topPercent).ToList();
            jgScores = jgScores.OrderByDescending(t => t.Item2).Take(topPercent).ToList();
            midScores = midScores.OrderByDescending(t => t.Item2).Take(topPercent).ToList();
            adcScores = adcScores.OrderByDescending(t => t.Item2).Take(topPercent).ToList();
            supScores = supScores.OrderByDescending(t => t.Item2).Take(topPercent).ToList();

            foreach (Characters character in characters)
            {
                if (topScores.Exists(t => t.Item1.Name == character.Name))
                    character.canTop = true;

                if (jgScores.Exists(t => t.Item1.Name == character.Name))
                    character.canJg = true;

                if (midScores.Exists(t => t.Item1.Name == character.Name))
                    character.canMid = true;

                if (adcScores.Exists(t => t.Item1.Name == character.Name))
                    character.canAdc = true;

                if (supScores.Exists(t => t.Item1.Name == character.Name))
                    character.canSup = true;
            }

/*            foreach (Tuple<Characters, double> tuple in supScores)
            {
                Debug.Log(tuple.Item1.Name);
            }*/

                //Sort from highest to lowest
            for (int loops = 0; loops < 1; loops++)
            {
                pickScores = 0;
                banScores = 0;
                int matches = 30000;
                int match = 0;
                //List<Items> charactersOG = FromJson(characterFile2);
                //Using the temporary stats list I "initialized" This is filling in and giving the respected values for them
                //TL:DR THis just sets the scores for the characters.
                foreach (TemporaryStats tempStat in temp)
                {
                    tempStat.pickStart = pickScores;
                    tempStat.banStart = banScores;
                    Characters character = characters.Find(c => c.Name == tempStat.name);
                    Stats characterStats = stats.Find(s => s.name == tempStat.name);
                    if (character != null)
                    {
                        tempStat.pickStart = pickScores;
                        pickScores += character.InitPickRate(characterStats);
                        tempStat.pickEnd = pickScores;
                        tempStat.banStart = banScores;
                        banScores += character.InitBanRate(characterStats);
                        tempStat.banEnd = banScores;
                    }
                }
                //This is the acutal looper to simulate the matches
                var progress = 0;
                Parallel.For(0, matches, new ParallelOptions { MaxDegreeOfParallelism = threads }, match =>
                {
                    String Elo = getElo(); // Sets match of the match
                    List<Characters> bans = Phases.Bans(characters, temp, Elo);
                    Team[] teams = Phases.Picks(characters, temp, Elo, bans);
                    Match Match = new Match(teams, Elo, stats, temp,characters);
                    Match.StartMatch();
                    progress++;
                    if (progress % (matches) == 0)
                    {
                        Debug.Log("%" + Math.Round(Divide(progress, matches) * 100, 2));
/*                        Debug.Log($"==============");
                        foreach (Team team in teams)
                        {
                            Debug.Log($"{team.top.Name}");
                            Debug.Log($"{team.jg.Name}");
                            Debug.Log($"{team.mid.Name}");
                            Debug.Log($"{team.adc.Name}");
                            Debug.Log($"{team.sup.Name}");
                            Debug.Log($"    - vs -");
                        }*/
                    }
                });



                //Finished everythign just looping and printing the stuff for you : ) 
                foreach (TemporaryStats tempStat in temp)
                {
                    Characters character = characters.Find(c => c.Name == tempStat.name);
                    double bans = ((tempStat.bans / matches) * 100);
                    double picks = ((tempStat.picks / matches) * 100);
                    double T = ((Divide(tempStat.pickTop, tempStat.picks)) * 100);
                    double J = ((Divide(tempStat.pickJungle, tempStat.picks)) * 100);
                    double M = ((Divide(tempStat.pickMid, tempStat.picks)) * 100);
                    double A = ((Divide(tempStat.pickADC, tempStat.picks)) * 100);
                    double S = ((Divide(tempStat.pickSupport, tempStat.picks)) * 100);

                    Debug.Log("-==============-");
                    Debug.Log(tempStat.name);
                    Debug.Log($"{Math.Round(tempStat.Kills/tempStat.picks,1)}/{Math.Round(tempStat.Deaths / tempStat.picks)}/{Math.Round(tempStat.Assists / tempStat.picks)}");
                    Debug.Log("Winrate  " + Math.Round(Divide(character.wins, tempStat.picks)*100,2) + "%");
                    Debug.Log("PickRate " + Math.Round(picks, 2) + "%");
                    Debug.Log("BanRate  " + Math.Round(bans, 2) + "%");
                    Debug.Log("Presence " + Math.Round(picks + bans, 2) + "%");
                    Debug.Log("Top " + Math.Round(T, 2) + "%");
                    Debug.Log("Jungle " + Math.Round(J, 2) + "%");
                    Debug.Log("Mid " + Math.Round(M, 2) + "%");
                    Debug.Log("ADC " + Math.Round(A, 2) + "%");
                    Debug.Log("Support " + Math.Round(S, 2) + "%");
                }
            }
            Console.ReadKey();
        }
        private string getElo()
        {
            double randomValue = getRandom();
            if (randomValue < 85)
            {
                return "Average";
            }
            else if (randomValue >= 85 && randomValue < 98)
            {
                return "Skilled";
            }
            else
            {
                return "Elite";
            }
        }

        private double getRandom()
        {
            Random random = new Random();
            return random.NextDouble() * 100;
        }

        //For no reason the division operator didnt work for somethings but this works...
        private double Divide(double a, double b)
        {
            if (b != 0)
            {
                return a / b;
            }
            else
            {
                return 0;
            }
        }
        //Top percentiles can do a specific role.
    }
}