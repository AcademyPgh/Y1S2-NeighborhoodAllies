using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string name { get; set; }
        public virtual Entity entity { get; set; }
        public string description { get; set; }
        public virtual Investment investmentIn { get; set; }
        public virtual Investment investmentOut { get; set; }
        public virtual Report report { get; set; }
    }
}