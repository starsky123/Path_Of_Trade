using Path_Of_Trade.TradeAPI.POE2.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.Item
{
    public class ItemStats 
    {

        public bool selected { get; set; }=false;
        public decimal min { get; set; }
        public decimal max { get; set; }
        public string text { get; set; }
        public string type { get; set; }
    }
}
