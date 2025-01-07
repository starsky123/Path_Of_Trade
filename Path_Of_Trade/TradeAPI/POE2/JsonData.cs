using Path_Of_Trade.TradeAPI.POE2.Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2
{
    internal class JsonData
    {
        public TradeQuery query { get; set; } = new();
        public SortType sort { get; set; } = new();
    }
}
