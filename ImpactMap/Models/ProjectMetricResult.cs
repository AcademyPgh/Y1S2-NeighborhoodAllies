using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class ProjectMetricResult
    {
        public int projectID { get; set; }
        public int metricID { get; set; }
        public string metricValue { get; set; }
    }
}