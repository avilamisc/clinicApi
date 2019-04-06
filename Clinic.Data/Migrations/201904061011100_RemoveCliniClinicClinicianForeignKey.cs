namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCliniClinicClinicianForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClinicClinicians", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.ClinicClinicians", new[] { "ClinicId" });
            AddColumn("dbo.Clinics", "ClinicName", c => c.String());
            DropColumn("dbo.ClinicClinicians", "ClinicId");
            DropColumn("dbo.Clinics", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clinics", "Name", c => c.String());
            AddColumn("dbo.ClinicClinicians", "ClinicId", c => c.Int(nullable: false));
            DropColumn("dbo.Clinics", "ClinicName");
            CreateIndex("dbo.ClinicClinicians", "ClinicId");
            AddForeignKey("dbo.ClinicClinicians", "ClinicId", "dbo.Clinics", "Id", cascadeDelete: true);
        }
    }
}
