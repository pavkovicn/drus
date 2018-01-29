namespace Server.Migrations
{
    using Server.Model.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Server.Model.Context.DRUSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Server.Model.Context.DRUSContext context)
        {

           

            if (context.Locations.Count()== 0) {
                Location l1 = new Location();
                Location l2 = new Location();
                Location l3 = new Location();
                l1.Name = "L1";
                l2.Name = "L2";
                l3.Name = "L3";
                context.Locations.Add(l1);
                context.Locations.Add(l2);
                context.Locations.Add(l3);
            }
            
        }
    }
}
