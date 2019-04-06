namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InheritClinicFromUser : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Clinics");
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ClinicName = c.String(),
                        City = c.String(nullable: false),
                        Geolocation = c.Geography(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clinics", "Id", "dbo.Users");
            DropIndex("dbo.Clinics", new[] { "Id" });
            DropTable("dbo.Clinics");
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClinicName = c.String(),
                        City = c.String(nullable: false),
                        Geolocation = c.Geography(),
                    })
                .PrimaryKey(t => t.Id);
        }
    }
}
