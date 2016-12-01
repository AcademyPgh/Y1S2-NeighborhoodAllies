using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImpactMap.Models
{
    public class ImpactMapDBContext : DbContext
    {
        public DbSet<Entity> entities { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<Investment> investments { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Metric> metrics { get; set; }
    }
}