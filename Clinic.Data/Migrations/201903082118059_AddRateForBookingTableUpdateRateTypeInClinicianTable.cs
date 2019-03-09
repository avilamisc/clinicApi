namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRateForBookingTableUpdateRateTypeInClinicianTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Rate", c => c.Single());
            AlterColumn("dbo.Bookings", "ClinicClinicianId", c => c.Int(nullable: false));
            AlterColumn("dbo.Bookings", "PatientId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClinicClinicians", "ClinicId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClinicClinicians", "ClinicianId", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "BookingId", c => c.Int());
            AlterColumn("dbo.RefreshTokens", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Clinicians", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Clinicians", "Rate", c => c.Single(nullable: false));
            AlterColumn("dbo.Patients", "Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Clinicians", "Rate", c => c.Int(nullable: false));
            AlterColumn("dbo.Clinicians", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.RefreshTokens", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "BookingId", c => c.Int());
            AlterColumn("dbo.Documents", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClinicClinicians", "ClinicianId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClinicClinicians", "ClinicId", c => c.Int(nullable: false));
            AlterColumn("dbo.Bookings", "PatientId", c => c.Int(nullable: false));
            AlterColumn("dbo.Bookings", "ClinicClinicianId", c => c.Int(nullable: false));
            DropColumn("dbo.Bookings", "Rate");
        }
    }
}
