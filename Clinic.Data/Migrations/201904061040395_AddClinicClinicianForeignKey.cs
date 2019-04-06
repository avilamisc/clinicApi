namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClinicClinicianForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClinicClinicians", "ClinicId", c => c.Int(nullable: false));
            CreateIndex("dbo.ClinicClinicians", "ClinicId");
            AddForeignKey("dbo.ClinicClinicians", "ClinicId", "dbo.Clinics", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClinicClinicians", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.ClinicClinicians", new[] { "ClinicId" });
            DropColumn("dbo.ClinicClinicians", "ClinicId");
        }
    }
}
