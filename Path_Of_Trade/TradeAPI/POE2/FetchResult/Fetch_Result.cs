using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path_Of_Trade.TradeAPI.POE2.FetchResult.FetchItem;
using Path_Of_Trade.TradeAPI.POE2.FetchResult.FetchList;

namespace Path_Of_Trade.TradeAPI.POE2.FetchResult
{
    internal class Fetch_Result
    {
        public string id { get; set; }
        public Fetch_Item item { get; set; } 
        public Fetch_List listing { get; set; }
    }
}
