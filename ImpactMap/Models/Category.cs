using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Display(Name = "Tag")]
        public string name { get; set; }
        public bool isBase { get; set; }
        //ID of the entity owning a user category, will be null if it's a base category
        public int? entityID { get; set; }
        //ID of the base category for a user category, will be null if it's a base category
        public int? baseID { get; set; }
        [Display(Name = "Metrics")]
        public virtual List<Metric> metrics { get; set; }
    }
}