namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DRUS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Measurements",
                c => new
                    {
                        MeasurerId = c.Guid(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Temperature = c.Int(nullable: false),
                        Humidity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MeasurerId, t.Time })
                .ForeignKey("dbo.Measurers", t => t.MeasurerId, cascadeDelete: true)
                .Index(t => t.MeasurerId);
            
            CreateTable(
                "dbo.Measurers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        LocationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Measurements", "MeasurerId", "dbo.Measurers");
            DropForeignKey("dbo.Measurers", "LocationId", "dbo.Locations");
            DropIndex("dbo.Measurers", new[] { "LocationId" });
            DropIndex("dbo.Measurements", new[] { "MeasurerId" });
            DropTable("dbo.Measurers");
            DropTable("dbo.Measurements");
            DropTable("dbo.Locations");
        }
    }
}
