using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Of_Trade
{
    internal class LeagueResult
    {
        public string id { get; set; }
        public string realm { get; set; }
        public string text { get; set; }
    }
    internal class League
    {
        public List<LeagueResult> result { get;set; }
    }
}
