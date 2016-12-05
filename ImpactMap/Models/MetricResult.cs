using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class MetricResult
    {
        public int ID { get; set; }
        public string resultText { get; set; }
        public virtual Report report { get; set; }
        public virtual Metric metric { get; set; }
    }
}