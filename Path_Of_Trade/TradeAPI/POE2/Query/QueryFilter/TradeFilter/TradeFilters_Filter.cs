using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.TradeFilter
{
    internal class TradeFilters_Filter
    {
        public Option indexed { get; set; }
        public Input account { get; set; }
        public Price price { get; set; }
        public Option sale_type { get; set; }
        public Option collapse { get; set; } 

    }
}
