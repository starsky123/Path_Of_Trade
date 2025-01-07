using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade.TradeAPI.POE2.FetchResult.FetchList
{
    internal class Fetch_List
    {
        //public Fetch_Account account { get; set; }
        public string indexed { get; set; }
        public string method { get; set; }
        public Fetch_Price price { get; set; }
        //public Fetch_Stash stash { get; set; }
        public string whisper { get; set; }
        public string whisper_token { get; set; }
    }
}
