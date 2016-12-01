using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int entityID { get; set; }
        public virtual List<Metric> metrics { get; set; }
    }
}