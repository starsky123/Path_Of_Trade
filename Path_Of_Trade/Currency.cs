using Path_Of_Trade.TradeAPI.POE2.POEDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade
{
    internal class Currency
    {
        public string name_EN { get; set; }
        public string name_TR { get; set; }
        public Currency(string c)
        {
            if (c == "Any")
            {
                name_EN = c;
                name_TR = c;
            }
            else
            {
                name_EN = Trade_Dictionary.TradeStaticDictionary[c];
                name_TR = Translate_Dictionary.currency[c];
            }
        }
    }
}
