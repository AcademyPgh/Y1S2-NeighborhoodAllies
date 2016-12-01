using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Investment
    {
        public int ID { get; set; }
        public string amount { get; set; }
        public int entityFrom { get; set; }
        public int entityTo { get; set; }
        public int projectFrom { get; set; }
        public int projectTo { get; set; }
        public DateTime date { get; set; }
        public bool isInKind { get; set; }
        public string volunteerHours { get; set; }
        public virtual List<Category> categories { get; set; }
    }
}