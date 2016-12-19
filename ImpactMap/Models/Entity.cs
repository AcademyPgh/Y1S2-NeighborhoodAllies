using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ImpactMap.Models
{
    public class Entity
    {
        public int ID { get; set; }
        [Display(Name = "Organization")]
        public string name { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }
        [Display(Name = "Company Logo")]
        public string logoURL { get; set; }
        [Display(Name = "Address Line 1")]
        public string address1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string address2 { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "ZIP Code")]
        public string zip { get; set; }
        [Display(Name = "Latitude")]
        public string lat { get; set; }
        [Display(Name = "Longitude")]
        public string lng { get; set; }
        public virtual List<User> users { get; set; }
        public virtual List<Project> projects { get; set; }
        public virtual List<Investment> investmentsIn { get; set; }
        public virtual List<Investment> investmentsOut { get; set; }
    }
}