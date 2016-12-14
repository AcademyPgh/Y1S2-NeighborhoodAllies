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
        public virtual List<Metric> metrics { get; set; }
    }
}