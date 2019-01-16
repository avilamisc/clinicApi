namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePatientAndClinicClinicianOptionalInBookingTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "ClinicClinicianId", "dbo.ClinicClinicians");
            DropIndex("dbo.Bookings", new[] { "ClinicClinicianId" });
            DropIndex("dbo.Bookings", new[] { "PatientId" });
            AlterColumn("dbo.Bookings", "ClinicClinicianId", c => c.Int(nullable: false));
            AlterColumn("dbo.Bookings", "PatientId", c => c.Int(nullable: false));
            CreateIndex("dbo.Bookings", "ClinicClinicianId");
            CreateIndex("dbo.Bookings", "PatientId");
            AddForeignKey("dbo.Bookings", "ClinicClinicianId", "dbo.ClinicClinicians", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "ClinicClinicianId", "dbo.ClinicClinicians");
            DropIndex("dbo.Bookings", new[] { "PatientId" });
            DropIndex("dbo.Bookings", new[] { "ClinicClinicianId" });
            AlterColumn("dbo.Bookings", "PatientId", c => c.Int());
            AlterColumn("dbo.Bookings", "ClinicClinicianId", c => c.Int());
            CreateIndex("dbo.Bookings", "PatientId");
            CreateIndex("dbo.Bookings", "ClinicClinicianId");
            AddForeignKey("dbo.Bookings", "ClinicClinicianId", "dbo.ClinicClinicians", "Id");
        }
    }
}
