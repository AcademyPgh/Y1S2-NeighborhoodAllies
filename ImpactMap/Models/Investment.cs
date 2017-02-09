using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Investment
    {
        public int ID { get; set; }
        [Display(Name = "Value of Grant")]
        [Required]
        public string amount { get; set; }
        public int? entityFrom_ID { get; set; }
        public int? entityTo_ID { get; set; }
        [Display(Name = "Donor")]
        [ForeignKey("entityFrom_ID")]
        [InverseProperty("investmentsOut")]
        public virtual Entity entityFrom { get; set; }
        [Display(Name = "Recipient")]
        [ForeignKey("entityTo_ID")]
        [InverseProperty("investmentsIn")]
        public virtual Entity entityTo { get; set; }
        [Display(Name = "Investment Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [Display(Name = "In Kind")]
        public bool isInKind { get; set; }
        [Display(Name = "Volunteer Hours")]
        public string volunteerHours { get; set; }
        [Display(Name = "Tags")]
        public virtual List<Category> categories { get; set; }
        public int? projectFrom_ID { get; set; }
        public int? projectTo_ID { get; set; }
        [Display(Name = "Project To")]
        [ForeignKey("projectTo_ID")]
        [InverseProperty("investmentsIn")]
        public virtual Project projectTo { get; set; }
        [Display(Name = "Project From")]
        [ForeignKey("projectFrom_ID")]
        [InverseProperty("investmentsOut")]
        public virtual Project projectFrom { get; set; }
        [Display(Name = "Investment Description")]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }
    }
}