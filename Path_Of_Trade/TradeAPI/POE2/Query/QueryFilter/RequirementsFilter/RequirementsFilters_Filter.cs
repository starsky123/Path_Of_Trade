using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.RequirementsFilter
{
    internal class RequirementsFilters_Filter
    {
        public MinMaxFilter dex { get; set; }
        public MinMaxFilter @int { get; set; }
        public MinMaxFilter str { get; set; }
        public MinMaxFilter lvl { get; set; }
    }
}
