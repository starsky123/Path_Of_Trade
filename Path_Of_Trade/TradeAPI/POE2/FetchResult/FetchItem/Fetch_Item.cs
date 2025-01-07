using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.FetchResult.FetchItem
{
    internal class Fetch_Item
    {
        public string baseType { get; set; }
        public List<string> explicitMods { get; set; }
        //public Fetch_Extended extended { get; set; } 
        public int frameType { get; set; }
        public int h { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public bool identified { get; set; }
        public int ilvl { get; set; }
        public List<string> implicitMods { get; set; }
        public string league { get; set; }
        public string name { get; set; }
        //public List<Fetch_Properties> properties { get; set; }
        public string rarity { get; set; }
        public string realm { get; set; }
        //public List<Fetch_Requirements> requirements { get; set; }
        public string typeLine { get; set; }
        public bool verified { get; set; }
        public int w { get; set; }


    }
}
