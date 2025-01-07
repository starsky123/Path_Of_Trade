using Path_Of_Trade.TradeAPI.POE2;
using Path_Of_Trade.TradeAPI.POE2.POEDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Path_Of_Trade.Item
{
    public class Equipment
    {
        const string labelpattern = @"\d+(\.\d+)?";
        const string statspattern = @"[+\-]?\d+(\.\d+)?";
        public string? name { get; set; }
        public string? type { get; set; }
        public bool currupted { get; set; } = false;
        public List<ItemLabel> Itemlabel = new();
        public List<ItemStats> Itemstats = new();
        public ItemStats GetStats(string stats)
        {
            string[] temp=stats.Split(new string[] { " (" }, StringSplitOptions.None);
            ItemStats itemStats = new ItemStats();

            MatchCollection matches = Regex.Matches(temp[0], statspattern);
            if (matches.Count > 0)
            {
                List<decimal> value= new List<decimal>();
                foreach (Match match in matches)
                {
                    value.Add(Convert.ToDecimal(match.Value));
                }
                itemStats.词缀 = Regex.Replace(temp[0], statspattern, "{0}");
                //itemStats.avg=itemStats.value.Average();
                decimal avg = value.Average();
                itemStats.最小值 = decimal.Round(avg * Settings1.Default.minroll / 100, GetPlaces(avg));
                itemStats.最大值 = decimal.Round(avg * Settings1.Default.maxroll / 100, GetPlaces(avg));
                if (temp.Length == 1)
                    itemStats.类型 = "explicit";
                else itemStats.类型 = temp[1].Replace(")", "");
            }
            if (itemStats.词缀 == null)
                return null;
            else
                return itemStats;
        }
        public ItemLabel GetLabel(string label) 
        {
            ItemLabel itemLabel = new ItemLabel();
            if (label.Contains(':'))
            {
                string[] temp= label.Split(':');
                itemLabel.标签 = temp[0].Split('（')[0].Split('(')[0];
                MatchCollection matches = Regex.Matches(temp[1], labelpattern);
                decimal sum = 0;
                foreach (Match match in matches)
                {
                    sum +=Convert.ToDecimal(match.Value);
                }
                if (matches.Count > 0)
                {
                    itemLabel.值 = (sum / (matches.Count > 1 ? 2 : 1)).ToString();
                }
                else
                    itemLabel.值 = temp[1].Replace(" ", "").Replace("\r\n", "");
                
            }
            return itemLabel;
        }
        public bool GetItemInfo(string text)
        {
            try
            {
                string[] temp = text.Replace("\r\n\r\n","\r\n").Replace("\r","").Replace("--------\n","").Split('\n');
                Itemlabel.Add(GetLabel(temp[0]));
                Itemlabel.Add(GetLabel(temp[1]));
                if (IsPOEItem())
                {
                    for (int i = 2; i < temp.Length-1; i++)
                    {
                        if (temp[i] != "")
                        {
                            if (temp[i].Contains(':'))
                                Itemlabel.Add(GetLabel(temp[i]));
                            else if (temp[i] == Translate_Dictionary.translate.FirstOrDefault(s=>s.Value== "ItemPopupCorrupted").Key)
                                currupted=true;
                            else
                            {
                                if (GetStats(temp[i]) != null)
                                    Itemstats.Add(GetStats(temp[i]));
                            }
                        }
                    }
                    if (Translate_Dictionary.translate[Itemlabel[1].值] == "ItemDisplayStringNormal")
                        type = temp[2];
                    else if(Translate_Dictionary.translate[Itemlabel[1].值] == "ItemDisplayStringMagic")
                        name= temp[2];
                    else
                    {
                        name = temp[2];
                        type = temp[3];
                    }
                    return true;
                }
                else { return false; }
            }
            catch { return false; }
        }
        public bool IsPOEItem()
        {

            if (Translate_Dictionary.translate[Itemlabel[0].标签]== "ItemFilterRuleItemClasses" && Translate_Dictionary.translate[Itemlabel[1].标签] == "ItemDisplayStringRarity")
            {
                return true;
            }
            else return false;
        }
        public int GetPlaces(decimal d)
        { 
            string s=d.ToString();
            if (s.Contains("."))
                return s.Split('.')[1].Length;
            else return 0;
        }

    }
}
