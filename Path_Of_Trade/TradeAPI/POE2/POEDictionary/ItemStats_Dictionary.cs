using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Windows;
using Path_Of_Trade.TradeAPI.POE2.Query;
using System.Text.RegularExpressions;

namespace Path_Of_Trade.TradeAPI.POE2.POEDictionary
{
    internal class ItemStats_Dictionary
    {
        public static Dictionary<LangStats,string> ItemStatsDictionary = new();
        public static void SetItemStatsDictionary()
        {
            try
            {
                string statstext = File.ReadAllText(@"TradeAPI\POE2\POEDictionary\stat_descriptions.csd");
                string[] temp = statstext.Replace("{1}","{0}").Replace("{}","{0}").Replace("description ", "description").Replace("\r\n\r\n", "\r\n").Split(new string[] { "description\r\n" }, StringSplitOptions.None);
                for (int i = 1; i < temp.Length; i++)
                {
                    if (temp[i] != "" && temp[i] != "\r\n")
                    {                       
                        temp[i]=temp[i].Insert(temp[i].IndexOf("\r\n") , "\r\n\tlang \"English\"");
                        string[] temp1 = temp[i].Split(new string[] { "lang " }, StringSplitOptions.None);
                        
                        List<string> valuelist = new List<string>();
                        for (int j = 1; j < temp1.Length; j++)
                        {
                            
                            string[] temp2 = temp1[j].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            string lang = temp2[0].Split("\"")[1];
                            int num = Convert.ToInt32(temp2[1].Replace("\t", ""));
                            //if (valuelist.Count>num)
                            //    MessageBox.Show(temp1[0]);
                            //string text = temp2[2].Split("\"")[1].Replace("0:+d", "0").Replace("\t", "");
                            //if (lang == "English")
                            //    value = text.Replace("{0}", "#");
                            //else
                            //{
                            //    string p = @"\[(.*?)\]";
                            //    MatchCollection matches = Regex.Matches(text, p);
                            //    foreach (Match match in matches)
                            //    {
                            //        if (match.Groups[1].Value.Contains('|'))
                            //        {
                            //            string t = match.Groups[1].Value.Split('|')[1];
                            //            text = text.Replace("[" + match.Groups[1].Value + "]", t);
                            //        }
                            //    }
                            //}
                            //langStats.language = lang;
                            //langStats.text = text;
                            //ItemStatsDictionary.Add(langStats, value);
                            

                            for (int k = 0; k < num; k++)
                            {
                                string text = temp2[2 + k].Split("\"")[1].Replace("0:+d", "0").Replace("\t", "");
                                if (lang == "English")
                                    valuelist.Add(text.Replace("{0}", "#"));
                                else
                                {
                                    string p = @"\[(.*?)\]";
                                    MatchCollection matches = Regex.Matches(text, p);
                                    foreach (Match match in matches)
                                    {
                                        if (match.Groups[1].Value.Contains('|'))
                                        {
                                            string t = match.Groups[1].Value.Split('|')[1];
                                            text = text.Replace("[" + match.Groups[1].Value + "]", t);
                                        }
                                    }
                                }
                                LangStats langStats = new LangStats();
                                langStats.language = lang;
                                langStats.text = text;

                                if (k < valuelist.Count)
                                    ItemStatsDictionary.TryAdd(langStats, valuelist[k]);

                            }

                        }
                    }
                }

            }
            catch (Exception er) { MessageBox.Show(er.Message); }
        }
    }
}
