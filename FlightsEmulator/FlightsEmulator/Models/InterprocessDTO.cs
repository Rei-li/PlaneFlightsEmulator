using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FlightsEmulator
{
   public class InterprocessDTO
    {
       public string MemoryMappedFileName { set; get; }
       public string MutexName { set; get; }
        public string JsonOut()
        {
            //Return json
            return JsonConvert.SerializeObject(this);
        }
    }
}
