namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialModelsEntitities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reciept = c.String(),
                        Name = c.String(nullable: false),
                        ClinicClinicianId = c.Int(),
                        PatientId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClinicClinicians", t => t.ClinicClinicianId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.ClinicClinicianId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.ClinicClinicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClinicId = c.Int(nullable: false),
                        ClinicianId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinics", t => t.ClinicId, cascadeDelete: true)
                .ForeignKey("dbo.Clinicians", t => t.ClinicianId)
                .Index(t => t.ClinicId)
                .Index(t => t.ClinicianId);
            
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Content = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Clinicians",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Region = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            DropTable("dbo.Laptops");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Laptops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Brand = c.String(),
                        Model = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Patients", "Id", "dbo.Users");
            DropForeignKey("dbo.Clinicians", "Id", "dbo.Users");
            DropForeignKey("dbo.Bookings", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Bookings", "ClinicClinicianId", "dbo.ClinicClinicians");
            DropForeignKey("dbo.ClinicClinicians", "ClinicianId", "dbo.Clinicians");
            DropForeignKey("dbo.Documents", "UserId", "dbo.Users");
            DropForeignKey("dbo.ClinicClinicians", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.Patients", new[] { "Id" });
            DropIndex("dbo.Clinicians", new[] { "Id" });
            DropIndex("dbo.Documents", new[] { "UserId" });
            DropIndex("dbo.ClinicClinicians", new[] { "ClinicianId" });
            DropIndex("dbo.ClinicClinicians", new[] { "ClinicId" });
            DropIndex("dbo.Bookings", new[] { "PatientId" });
            DropIndex("dbo.Bookings", new[] { "ClinicClinicianId" });
            DropTable("dbo.Patients");
            DropTable("dbo.Clinicians");
            DropTable("dbo.Documents");
            DropTable("dbo.Users");
            DropTable("dbo.Clinics");
            DropTable("dbo.ClinicClinicians");
            DropTable("dbo.Bookings");
        }
    }
}
