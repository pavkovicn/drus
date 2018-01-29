using Server.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.Context
{
    public class DRUSContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        public DbSet<Measurer> Measurers { get; set; }

        public DbSet<Measurement> Measurements { get; set; }
    }
}
