using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.StatFilter
{
    internal class StatFilters_Filter
    {

        public bool disabled { get; set; } = false;
        public string id { get; set; }
        public MinMaxFilter value { get; set; }
    }
}
