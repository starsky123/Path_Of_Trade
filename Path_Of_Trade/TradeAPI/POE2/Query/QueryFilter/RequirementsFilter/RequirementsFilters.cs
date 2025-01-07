using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.RequirementsFilter
{
    internal class RequirementsFilters
    {
        public bool disabled { get; set; } = false;
        public RequirementsFilters_Filter filters { get; set; } = new();
    }
}
