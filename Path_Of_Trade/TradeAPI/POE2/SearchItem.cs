using Newtonsoft.Json;
using Path_Of_Trade.Item;
using Path_Of_Trade.TradeAPI;
using Path_Of_Trade.TradeAPI.POE2.POEDictionary;
using Path_Of_Trade.TradeAPI.POE2.Query;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.TradeFilter;
using Path_Of_Trade.TradeAPI.POE2.Query.StatFilter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Path_Of_Trade.TradeAPI.POE2
{
    internal class SearchItem
    {
        public string ID { get; set; }
        public int Total { get; set; }
        public List<string> result { get; set; } = new();
        public static SearchItem Search(string league, string language,string name,string type,bool currupted,DataGrid tag, DataGrid stats, TradeFilters_Filter tradeFilters)
        {
            try
            {
                List<ItemLabel> itemLabels = (List<ItemLabel>)tag.ItemsSource;
                List<ItemStats> itemStats = (List<ItemStats>)stats.ItemsSource;
                JsonData item = new JsonData();
                //decimal min = Settings1.Default.minroll / 100;
                //decimal max = Settings1.Default.maxroll / 100;
                #region type_filter
                string category = Translate_Dictionary.translate[GetLabelValue(itemLabels, "ItemFilterRuleItemClasses", "string")];
                if (category!="")
                    item.query.filters.type_filters.filters.category = new Option(category);
                
                string rarity = GetLabelValue(itemLabels, "ItemDisplayStringRarity", "string").Replace("ItemDisplayString","").ToLower();
                if (rarity!="")
                    item.query.filters.type_filters.filters.rarity = new Option(rarity);
                decimal ilvl= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayStringItemLevel", "decimal"));
                if(ilvl>0)
                    item.query.filters.type_filters.filters.ilvl= new MinMaxFilter { min = ilvl };

                decimal quality = 0;
                ItemLabel label = itemLabels.SingleOrDefault(s => Translate_Dictionary.translate[s.标签] == "Quality" );
                if (label != null)
                {
                    quality = Convert.ToDecimal(label.值);
                }
                decimal maxquality = currupted ? 1 : (decimal)1.2/ (1 + quality / 100);
                #endregion
                #region equipment_filter
                if (category.Contains("weapon"))
                {
                    decimal pdph = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayWeaponPhysicalDamage", "decimal"));
                    decimal colddph= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayWeaponColdDamage", "decimal"));
                    decimal firedph= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayWeaponFireDamage", "decimal"));
                    decimal lightningdph= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayWeaponLightningDamage", "decimal"));
                    decimal edph = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayWeaponElementalDamage", "decimal"));
                    edph += colddph + firedph + lightningdph;
                    decimal chaosdph= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayWeaponChaosDamage", "decimal"));
                    decimal aps= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayWeaponAttacksPerSecond", "decimal"));
                    decimal crit= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplaySkillGemCriticalStrikeChance", "decimal"));
                    decimal spirit= Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplaySpiritValue", "decimal"));
                    decimal damage = pdph  * maxquality + edph + chaosdph;
                    decimal pdps = pdph  * maxquality * aps;
                    decimal edps = edph  * aps;
                    decimal dps = damage * aps;
                    if (dps > 0)
                    {
                        item.query.filters.equipment_filters.filters.dps = new MinMaxFilter { min = dps };
                        if (pdps > 0)
                            item.query.filters.equipment_filters.filters.pdps = new MinMaxFilter { min = pdps };
                        if (edps > 0)
                            item.query.filters.equipment_filters.filters.edps = new MinMaxFilter { min = edps };
                    }
                    else
                    {
                        if (damage > 0)
                            item.query.filters.equipment_filters.filters.damage = new MinMaxFilter { min = damage };
                        if (aps > 0)
                            item.query.filters.equipment_filters.filters.aps = new MinMaxFilter { min = aps };
                    }
                    if (crit > 0)
                        item.query.filters.equipment_filters.filters.crit = new MinMaxFilter { min = crit };
                    
                    if (spirit > 0)
                        item.query.filters.equipment_filters.filters.spirit = new MinMaxFilter { min = spirit };
                }
                else if (category.Contains("armour"))
                {
                    decimal ar = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayArmourArmour", "decimal"))  * maxquality;
                    decimal ev = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayArmourEvasionRating", "decimal"))  * maxquality;
                    decimal es = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayArmourEnergyShield", "decimal")) * maxquality;
                    decimal block = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayShieldBlockChance", "decimal"));
                    if (ar > 0)
                        item.query.filters.equipment_filters.filters.ar = new MinMaxFilter { min = ar };
                    if (ev > 0)
                        item.query.filters.equipment_filters.filters.ev = new MinMaxFilter { min = ev };
                    if (es > 0)
                        item.query.filters.equipment_filters.filters.es = new MinMaxFilter { min = es };
                    if (block > 0)
                        item.query.filters.equipment_filters.filters.damage = new MinMaxFilter { min = block };
                }

                #endregion
                #region req_filter
                decimal str = Convert.ToDecimal(GetLabelValue(itemLabels, "Strength", "decimal"));
                decimal dex = Convert.ToDecimal(GetLabelValue(itemLabels, "Dexterity", "decimal"));
                decimal @int = Convert.ToDecimal(GetLabelValue(itemLabels, "Intelligence", "decimal"));
                decimal lvl= Convert.ToDecimal(GetLabelValue(itemLabels, "Level", "decimal"));
                if (str > 0)
                    item.query.filters.req_filters.filters.str= new MinMaxFilter { min = str };
                if (dex > 0)
                    item.query.filters.req_filters.filters.dex = new MinMaxFilter { min = dex };
                if (@int > 0)
                    item.query.filters.req_filters.filters.@int = new MinMaxFilter { min = @int };
                if (lvl > 0)
                    item.query.filters.req_filters.filters.lvl = new MinMaxFilter { min = lvl };
                #endregion
                #region waystone_filter
                decimal tier = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayMapTier", "decimal"));
                decimal bonus = Convert.ToDecimal(GetLabelValue(itemLabels, "ItemDisplayMapMapItemDropChanceIncrease", "decimal"));
                if (tier > 0)
                    item.query.filters.map_filters.filters.map_tier = new MinMaxFilter { min = tier };
                if(bonus>0)
                    item.query.filters.map_filters.filters.map_bonus=new MinMaxFilter { min = bonus };
                #endregion
                #region trade_filter
                item.query.filters.trade_filters.filters = tradeFilters;
                #endregion
                #region stats
                StatFilters statFilters = new StatFilters();
                statFilters.type = "and";
                for (int i = 0; i < itemStats.Count; i++)
                {
                    if (itemStats[i].选择)
                    {
                        string id = ItemStatsToTrade(itemStats[i], language);
                        if (id != "")
                        {
                            statFilters.filters.Add(new StatFilters_Filter()
                            {
                                id = id,
                                value = new MinMaxFilter() { min = itemStats[i].最小值, max = itemStats[i].最大值 }
                            });
                        }
                        else itemStats[i].选择 = false;
                    }
                }         
                item.query.stats.Add(statFilters);
                stats.ItemsSource = null;
                stats.ItemsSource = itemStats;
                #endregion
                #region name_type
                if (rarity == "unique")
                    item.query.name = Translate_Dictionary.translate[name];
                if (type != "")
                    item.query.type = Translate_Dictionary.translate[type];
                #endregion

                string json = JsonConvert.SerializeObject(item, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
                string result = SendHTTP.Post(SendHTTP.SearchApi[1] + league, 100, json);
                return JsonConvert.DeserializeObject<SearchItem>(result);

            }
            catch (Exception er) { MainWindow.messageshow(er.Message); return null; }
        }
        public static string ItemStatsToTrade(ItemStats itemStats, string language)
        {
            string tradestats = "";
            var trade = from i in ItemStats_Dictionary.ItemStatsDictionary
                        where i.Key.language == language && i.Key.text == itemStats.词缀
                        join t in Trade_Dictionary.TradeStatsDictionary on i.Value equals t.Key.text
                        where t.Key.type == itemStats.类型
                        select t.Value;
            tradestats = trade.FirstOrDefault() ?? "";
            return tradestats;
        }
        public static string GetLabelValue(List<ItemLabel> itemLabels,string labelname,string type)
        {
            ItemLabel label = itemLabels.SingleOrDefault(s => Translate_Dictionary.translate[s.标签] == labelname && s.选择);
            if (type == "decimal")
            {
                if (label != null)
                    return label.值;
                else return "0";
            }
            else
            {
                if (label != null)
                    return Translate_Dictionary.translate[label.值];
                else return "";                
            }

        }
        public static int GetPlaces(decimal d)
        {
            string s = d.ToString();
            if (s.Contains("."))
                return s.Split('.')[1].Length;
            else return 0;
        }



    }
}
