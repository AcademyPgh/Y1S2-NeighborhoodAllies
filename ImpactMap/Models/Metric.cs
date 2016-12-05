using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Metric
    {
        public int ID { get; set; }
        public int categoryID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}