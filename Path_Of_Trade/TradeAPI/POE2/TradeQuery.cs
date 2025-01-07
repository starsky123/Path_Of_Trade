using Path_Of_Trade.TradeAPI.POE2.Query;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter;
using Path_Of_Trade.TradeAPI.POE2.Query.StatFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2
{
    internal class TradeQuery
    {
        public string name { get; set; }
        public string type { get; set; }
        public StatusFilters status { get; set; } = new();
        public List<StatFilters> stats { get; set; } = new();
        public QueryFilters filters { get; set; } = new();
    }
}
