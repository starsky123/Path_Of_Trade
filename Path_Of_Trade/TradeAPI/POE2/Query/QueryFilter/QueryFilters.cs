using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.TradeFilter;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.TypeFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.RequirementsFilter;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.MiscellaneousFilter;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.EquipmentFilter;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.WaystoneFilter;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter
{
    internal class QueryFilters
    {
        public MiscellaneousFilters misc_filters { get; set; } = new();
        public RequirementsFilters req_filters { get; set; } = new();
        public TradeFilters trade_filters { get; set; } = new();
        public TypeFilters type_filters { get; set; } = new();
        public EquipmentFilters equipment_filters { get; set; } = new();
        public WaystoneFilters map_filters { get; set; } = new();
    }
}
