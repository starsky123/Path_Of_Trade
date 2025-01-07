using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.EquipmentFilter
{
    internal class EquipmentFilters
    {
        public bool disabled { get; set; } = false;
        public EquipmentFilters_Filter filters { get; set; } = new();
    }
}
