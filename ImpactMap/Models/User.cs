using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class User
    {
        public int ID { get; set; }
        public Guid userModelGuid { get; set; }
        public string userModelName { get; set; }
    }
}