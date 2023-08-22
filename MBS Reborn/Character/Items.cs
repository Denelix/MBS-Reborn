using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS_Reborn.Character
{
    public class Items : Characters
    {
        public List<ItemStats> ItemStatsList { get; set; }

        public Items()
        {
            ItemStatsList = new List<ItemStats>();
        }

        public void SetItemStat(string characterName, string itemName, double winRate, double pickRate)
        {
            var existingItemStat = ItemStatsList.Find(stat =>
                stat.CharacterName == characterName && stat.ItemName == itemName);

            if (existingItemStat != null)
            {
                existingItemStat.WinRate = winRate;
                existingItemStat.PickRate = pickRate;
            }
            else
            {
                var newItemStat = new ItemStats
                {
                    CharacterName = characterName,
                    ItemName = itemName,
                    WinRate = winRate,
                    PickRate = pickRate
                };

                ItemStatsList.Add(newItemStat);
            }
        }

        public string ExportToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Items ImportFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Items>(json);
        }
        public void WriteToFile(string filePath)
        {
            string json = ExportToJson();
            File.WriteAllText(filePath, json);
        }
        public static Items ReadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return ImportFromJson(json);
        }
    }

    public class ItemStats
    {
            public string CharacterName { get; set; }
            public string ItemName { get; set; }
            public double WinRate { get; set; }
            public double PickRate { get; set; }
     }
}
