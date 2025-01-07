using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.TradeFilter
{
    internal class Price
    {
        public int? min { get; set; }
        public int? max { get; set; }
        public string? option { get; set; }
    }
}
