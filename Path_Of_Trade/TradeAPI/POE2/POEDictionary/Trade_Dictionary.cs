using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using static Path_Of_Trade.TradeAPI.POE2.POEDictionary.TradeStaticApi;

namespace Path_Of_Trade.TradeAPI.POE2.POEDictionary
{
    internal class Trade_Dictionary
    {
        public static Dictionary<TradeStats, string> TradeStatsDictionary = new();
        public static Dictionary<string,string> TradeStaticDictionary= new();
        public static void SetTradeDictionary()
        {
            try
            {
                string result = SendHTTP.Get(SendHTTP.StatsApi[1], 15);
                if (!string.IsNullOrEmpty(result))
                {
                    TradeStatsApi tradeStatsapi = new TradeStatsApi();
                    tradeStatsapi = JsonConvert.DeserializeObject<TradeStatsApi>(result);
                    for (int i = 0; i < tradeStatsapi.result.Count; i++)
                    {
                        StatsResult resultApis = tradeStatsapi.result[i];
                        for (int j = 0; j < resultApis.entries.Count; j++)
                        {
                            TradeStats tradeStats = new();
                            tradeStats.type = resultApis.entries[j].type;
                            tradeStats.text = resultApis.entries[j].text;
                            TradeStatsDictionary.TryAdd(tradeStats, resultApis.entries[j].id);
                        }
                    }
                }
                result = SendHTTP.Get(SendHTTP.StaticApi[1], 15);
                if (!string.IsNullOrEmpty(result))
                {
                    TradeStaticApi tradeStaticapi = new TradeStaticApi();
                    tradeStaticapi = JsonConvert.DeserializeObject<TradeStaticApi>(result);
                    for (int i = 0; i < tradeStaticapi.result.Count; i++)
                    {
                        StaticResult resultApis = tradeStaticapi.result[i];
                        for (int j = 0; j < resultApis.entries.Count; j++)
                        {
                            TradeStaticDictionary.TryAdd(resultApis.entries[j].text, resultApis.entries[j].id);
                        }
                    }
                }

            }
            catch (Exception er) { MainWindow.messageshow(er.Message); }
        }
    }
}
