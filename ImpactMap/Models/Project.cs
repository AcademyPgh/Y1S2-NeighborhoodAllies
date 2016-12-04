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
        public string entityID { get; set; }
        public string description { get; set; }
        public string investmentIn { get; set; }
        public string investmentOut { get; set; }
        public bool isPassThrough { get; set; }
        public virtual List<Metric> metrics { get; set; }
    }
}