using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.WaystoneFilter
{
    internal class WaystoneFilters_Filter
    {
        public MinMaxFilter map_bonus { get; set; }
        public MinMaxFilter map_tier { get; set; }
    }
}
