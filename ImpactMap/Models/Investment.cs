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
        [Display(Name = "Amount")]
        public string amount { get; set; }
        public int? entityFrom_ID { get; set; }
        public int? entityTo_ID { get; set; }
        [Display(Name = "Donor")]
        public virtual Entity entityFrom { get; set; }
        [Display(Name = "Recipient")]
        public virtual Entity entityTo { get; set; }
        [Display(Name = "Investment Date")]
        public DateTime date { get; set; }
        [Display(Name = "In Kind")]
        public bool isInKind { get; set; }
        [Display(Name = "Volunteer Hours")]
        public string volunteerHours { get; set; }
        [Display(Name = "Tags")]
        public virtual List<Category> categories { get; set; }
        public int? projectFrom_ID { get; set; }
        public int? projectTo_ID { get; set; }
        [Display(Name = "Project To:")]
        public virtual Project projectTo { get; set; }
        [Display(Name = "Project From:")]
        public virtual Project projectFrom { get; set; }
        [Display(Name = "Investment Description")]
        public string description { get; set; }
    }
}