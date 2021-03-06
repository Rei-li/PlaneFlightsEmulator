﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightsEmulator
{
    public class PathPoint
    {
        public int id { set; get; }
        public decimal lat { set; get; }
        public decimal lng { set; get; }
        public decimal alt { set; get; }
        public string JsonOut()
        {
            //Return json
            return JsonConvert.SerializeObject(this);
        }
    }
}
