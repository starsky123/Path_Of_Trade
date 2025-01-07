using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static Path_Of_Trade.TradeAPI.POE2.POEDictionary.TradeStatsApi;
using System.Windows;

namespace Path_Of_Trade.TradeAPI.POE2.POEDictionary
{
    internal class StatsEntries
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
    }
    internal class StatsResult
    {
        public string id { get; set; }
        public string label { get; set; }
        public List<StatsEntries> entries { get; set; }
    }
    internal class TradeStatsApi
    {       
        public List<StatsResult> result { get; set; } 
    }
}
