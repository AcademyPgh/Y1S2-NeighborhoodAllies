using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ImpactMap.Models
{
    public class Entity
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public virtual List<User> users { get; set; }
        public virtual List<Project> projects { get; set; }
        public virtual List<Investment> investmentsIn { get; set; }
        public virtual List<Investment> investmentsOut { get; set; }
    }
}