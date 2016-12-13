using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Report
    {
        public int ID { get; set; }
        [Display(Name = "Report")]
        public string name { get; set; }
        //[Required]
        //public virtual Project project { get; set; }
        public bool completed { get; set; }
        public string reportText { get; set; }
        public DateTime dueDate { get; set; }
        public virtual List<MetricResult> metricResults { get; set; }
    }
}