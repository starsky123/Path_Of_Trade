using Newtonsoft.Json;
using Path_Of_Trade.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Path_Of_Trade.TradeAPI.POE2.POEDictionary
{
    internal class Translate_Dictionary
    {
        public static Dictionary<string, string> translate = new()
        {
            {"Gloves","armour.gloves" } ,{"Boots","armour.boots" } ,{"Body Armour","armour.chest" } ,{"Helmet","armour.helmet" } ,
            {"Focus","armour.focus" } ,{"Quiver","armour.quiver" } ,{"Shield","armour.shield" } ,
            {"Claw","weapon.Claw" } ,{"Dagger","weapon.dagger" } ,{"Wand","weapon.wand" } ,{"One Hand Sword","weapon.onesword" } ,
            {"Thrusting One Hand Sword","weapon.thrustingonesword" } ,{"One Hand Axe","weapon.oneaxe" } ,{"One Hand Mace","weapon.onemace" } ,
            {"Sceptre","weapon.sceptre" } ,{"RuneDagger","weapon.runedagger" } ,{"Bow","weapon.bow" } ,{"Staff","weapon.staff" } ,
            {"Two Hand Sword","weapon.twosword" } ,{"Two Hand Axe","weapon.twoaxe" } ,{"Two Hand Mace","weapon.twomace" } ,{"Warstaff","weapon.warstaff" } ,
            {"Crossbow","weapon.crossbow" } ,
            {"Amulet","accessory.amulet" } ,{"Ring","accessory.ring" } ,{"Belt","accessory.belt" } ,{"Talisman","accessory.talisman" } ,
            {"Trinket","accessory.trinket" },
            {"Jewel","jewel" },
            { "Map","map.waystone"},{ "TowerAugmentation","map.tablet"},{"ExpeditionLogbook","map.logbook"},
            { "Relic","sanctum.relic"},
            { "LifeFlask","flask.life"},{"ManaFlask","flask.mana"},
            {"一口价","~price" },{"可议价","~b/o" }
          };
        public static Dictionary<string, string> currency = new();
        public static void Set(string language)
        {
            string text = File.ReadAllText(@"Json\" + language + @"\itemclasses.json");
            List<Itemclasses> itemclasses = JsonConvert.DeserializeObject<List<Itemclasses>>(text);
            foreach (Itemclasses c in itemclasses)
            {
                translate.TryAdd(c.Name, c.Id);
            }
            
            text = File.ReadAllText(@"Json\baseitemtypes_EN.json");
            List<Baseitemtypes> EN = JsonConvert.DeserializeObject<List<Baseitemtypes>>(text);
            text = File.ReadAllText(@"Json\" + language + @"\baseitemtypes.json");
            List<Baseitemtypes> lang = JsonConvert.DeserializeObject<List<Baseitemtypes>>(text);
            foreach (Baseitemtypes b in EN)
            {
                translate.TryAdd(lang.SingleOrDefault(s => s._rid == b._rid).Name, b.Name);
                if (b.ItemClassesKey == 30)
                    currency.TryAdd(b.Name, lang.SingleOrDefault(s => s._rid == b._rid).Name);
            }
            
            text = File.ReadAllText(@"Json\" + language + @"\clientstrings.json");
            List<Clientstrings> client = JsonConvert.DeserializeObject<List<Clientstrings>>(text);
            foreach (Clientstrings c in client)
            {
                string p = @"\[(.*?)\]";
                MatchCollection matches = Regex.Matches(c.Text, p);
                foreach (Match match in matches)
                {
                    if (match.Groups[1].Value.Contains('|'))
                    {
                        string t = match.Groups[1].Value.Split('|')[1];
                        c.Text = c.Text.Replace("[" + match.Groups[1].Value + "]", t);
                    }
                }
                translate.TryAdd(c.Text, c.Id);
            }
            text = File.ReadAllText(@"Json\" + language + @"\words.json");
            List<Words> word = JsonConvert.DeserializeObject<List<Words>>(text);
            foreach (Words w in word)
            {
                translate.TryAdd(w.Text2, w.Text);
            }
        }
    }
}
