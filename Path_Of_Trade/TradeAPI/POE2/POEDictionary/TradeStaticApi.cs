using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.POEDictionary
{
    internal class TradeStaticApi
    {
        internal class StaticEntries
        {
            public string id { get; set; }
            public string text { get; set; }
            public string image { get; set; }
        }
        internal class StaticResult
        {
            public string id { get; set; }
            public string label { get; set; }
            public List<StaticEntries> entries { get; set; }
        }
        public List<StaticResult> result { get; set; }
    }
}
