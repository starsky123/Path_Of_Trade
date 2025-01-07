using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.EquipmentFilter
{
    internal class EquipmentFilters_Filter
    {
        public MinMaxFilter ar { get; set; } = new();
        public MinMaxFilter ev { get; set; } = new();
        public MinMaxFilter es { get; set; } = new();
        public MinMaxFilter block { get; set; } = new();
        public MinMaxFilter damage { get; set; } = new();
        public MinMaxFilter aps { get; set; } = new();
        public MinMaxFilter crit { get; set; } = new();
        public MinMaxFilter dps { get; set; } = new();
        public MinMaxFilter pdps { get; set; } = new();
        public MinMaxFilter edps { get; set; } = new();
        public MinMaxFilter rune_sockets { get; set; } = new();
        public MinMaxFilter spirit { get; set; } = new();
    }
}
