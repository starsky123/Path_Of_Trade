using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.StatFilter
{
    internal class StatFilters
    {
        public bool disabled { get; set; } = false;
        public List<StatFilters_Filter> filters { get; set; } = new();
        public string type { get; set; }
    }
}
