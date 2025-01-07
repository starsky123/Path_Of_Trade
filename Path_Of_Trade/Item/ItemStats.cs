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

        public bool 选择 { get; set; }=false;
        public decimal 最小值 { get; set; }
        public decimal 最大值 { get; set; }
        public string 词缀 { get; set; }
        public string 类型 { get; set; }
    }
}
