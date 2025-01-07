using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Path_Of_Trade.TradeAPI.POE2.FetchResult;

namespace Path_Of_Trade.TradeAPI.POE2
{
    internal class Fetch
    {
        public List<Fetch_Result> result { get; set; }
        public static Fetch FetchItem(SearchItem searchItem,int num)
        {
            if (searchItem != null && searchItem.Total>0)
            {
                string sr = "";
                int fetchnum = num < searchItem.result.Count ? num : searchItem.result.Count;
                for (int i = 0; i < fetchnum; i++)
                {
                    sr += searchItem.result[i] + "%2c";
                }
                string url = SendHTTP.FetchApi[1] + sr + "?query=" + searchItem.ID;
                string result = SendHTTP.Get(url, 100);
                return JsonConvert.DeserializeObject<Fetch>(result);
            }
            else return null;
        }

    }
}
