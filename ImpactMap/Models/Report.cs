using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Report
    {
        public int ID { get; set; }
        public int projectID { get; set; }
        public bool completed { get; set; }
        public string reportText { get; set; }
        public DateTime dueDate { get; set; }
    }
}