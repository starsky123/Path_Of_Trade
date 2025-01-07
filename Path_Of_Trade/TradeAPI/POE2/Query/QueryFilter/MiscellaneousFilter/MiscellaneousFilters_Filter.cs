using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.MiscellaneousFilter
{
    internal class MiscellaneousFilters_Filter
    {
        public MinMaxFilter gem_level { get; set; }
        public MinMaxFilter gem_sockets { get; set; }
        public MinMaxFilter area_level  { get; set; }
        public MinMaxFilter sanctum_gold { get; set; }
        public MinMaxFilter unidentified_tier { get; set; }
        public MinMaxFilter stack_size { get; set; }
        public Option alternate_art { get; set; }
        public Option corrupted { get; set; }
        public Option identified { get; set; }
        public Option mirrored { get; set; }


    }
}
