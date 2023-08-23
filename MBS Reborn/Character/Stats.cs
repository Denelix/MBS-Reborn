using Newtonsoft.Json;

namespace MBS_Reborn.Character
{
    public class Stats
    {
        public string name { get; set; }
        public int picks { get; set; }
        public int wins { get; set; }
        public int loses { get; set; }
        public int bans { get; set; }
        public int matches { get; set; }
        public double winTimes { get; set; }
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

        public static Stats FromJson(string filePath)
        {
            return JsonConvert.DeserializeObject<Stats>(filePath);
        }

        public void WriteToFile(string filePath)
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, json);
        }
    }
}
