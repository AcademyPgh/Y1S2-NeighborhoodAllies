using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Investment
    {
        public int ID { get; set; }
        public string amount { get; set; }
        public int? entityFrom_ID { get; set; }
        public int? entityTo_ID { get; set; }
        public virtual Entity entityFrom { get; set; }
        public virtual Entity entityTo { get; set; }
        public DateTime date { get; set; }
        public bool isInKind { get; set; }
        public string volunteerHours { get; set; }
        public virtual List<Category> categories { get; set; }
        public int? projectFrom_ID { get; set; }
        public int? projectTo_ID { get; set; }
        public virtual Project projectTo { get; set; }
        public virtual Project projectFrom { get; set; }
        public string description { get; set; }
    }
}