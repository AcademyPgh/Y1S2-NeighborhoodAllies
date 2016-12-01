using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int entityID { get; set; }
        public string description { get; set; }
        public int investmentIn { get; set; }
        public int investmentOut { get; set; }
        public bool isPassThrough { get; set; }
        public virtual List<Metric> metrics { get; set; }
    }
}