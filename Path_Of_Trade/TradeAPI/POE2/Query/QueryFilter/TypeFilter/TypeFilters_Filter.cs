using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.TypeFilter
{
    internal class TypeFilters_Filter
    {
        public Option category { get; set; }
        public Option rarity { get; set; }
        public MinMaxFilter ilvl { get; set; }
        public MinMaxFilter quality { get; set; }


    }
}
