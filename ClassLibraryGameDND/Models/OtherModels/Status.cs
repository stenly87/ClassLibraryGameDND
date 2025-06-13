﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class Status
    {
        public string Info { get; set; }
        public string Reward { get; set; }
        public List<string> Events { get; set; } = new List<string>();
        public List<int> LogId { get; set; } = new List<int>();
    }
}
