using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class Status
    {
        public string Name { get; set; }
        public string HP { get; set; }
        public string Portrait { get; set; }
        public string Info { get; set; }
        public string Reward { get; set; }
        public string TimePass { get; set; }
        public double Progress { get; set; }
        public bool End { get; set; } = false;
        public List<string> Events { get; set; } = new List<string>();
        public List<int> LogId { get; set; } = new List<int>();
    }
}
