using Microsoft.Office.Interop.Excel;

namespace MBS_Reborn.Character
{
    public class TemporaryStats
    {
        public string name { get; set; }
        public double pickStart { get; set; }
        public double pickEnd { get; set; }
        public double banStart { get; set; }
        public double banEnd { get; set; }
        public double bans { get; set; }
        public double picks { get; set; }
        public bool canTop { get; set; }
        public bool canMid { get; set; }
        public bool canAdc { get; set; }
        public bool canJg { get; set; }
        public bool canSup { get; set; }
        public int pickTop { get; set; }
        public int pickMid { get; set; }
        public int pickADC { get; set; }
        public int pickJungle { get; set; }
        public int pickSupport { get; set; }
        public int winTop { get; set; }
        public int winMid { get; set; }
        public int winADC { get; set; }
        public int winJungle { get; set; }
        public int winSupport { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public override string ToString()
        {
            return $"Character: {name}";
        }
    }
}
