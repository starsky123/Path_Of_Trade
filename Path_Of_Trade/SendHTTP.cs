using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Path_Of_Trade
{
    internal class SendHTTP
    {
        public static string[] LeagueApi = { "https://www.pathofexile.com/api/trade/data/leagues", "https://www.pathofexile.com/api/trade2/data/leagues" };
        public static string[] StatsApi = { "https://www.pathofexile.com/api/trade/data/stats", "https://www.pathofexile.com/api/trade2/data/stats" };
        public static string[] StaticApi = { "https://www.pathofexile.com/api/trade/data/static", "https://www.pathofexile.com/api/trade2/data/static" }; 
        public static string[] SearchUrl = { "https://www.pathofexile.com/trade/search/", "https://www.pathofexile.com/trade2/search/poe2/" };
        public static string[] SearchApi = { "https://www.pathofexile.com/api/trade/search/", "https://www.pathofexile.com/api/trade2/search/" };
        public static string[] FetchApi = { "https://www.pathofexile.com/api/trade/fetch/", "https://www.pathofexile.com/api/trade2/fetch/" };
        public static string[] ExchangeApi = { "https://www.pathofexile.com/api/trade/exchange/", "https://www.pathofexile.com/api/trade2/exchange/" };
        public static string Post(string url, int timeout, string json)
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Timeout = timeout * 1000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2"; 

            if (json == null)
            {
                request.Method = WebRequestMethods.Http.Get;
            }
            else
            {
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("Content-Encoding", "utf-8");
                request.Method = WebRequestMethods.Http.Post;

                byte[] data = Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;
                request.GetRequestStream().Write(data, 0, data.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
        public static string Get(string url, int timeout)
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.Timeout = timeout * 1000;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
