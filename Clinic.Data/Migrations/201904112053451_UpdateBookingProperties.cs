namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookingProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "HeartRate", c => c.Short());
            AddColumn("dbo.Bookings", "Weight", c => c.Single());
            AddColumn("dbo.Bookings", "Height", c => c.Short());
            AddColumn("dbo.Bookings", "PatientDescription", c => c.String());
            AddColumn("dbo.Bookings", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bookings", "UpdateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Bookings", "Reciept");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "Reciept", c => c.String());
            DropColumn("dbo.Bookings", "UpdateDate");
            DropColumn("dbo.Bookings", "CreationDate");
            DropColumn("dbo.Bookings", "PatientDescription");
            DropColumn("dbo.Bookings", "Height");
            DropColumn("dbo.Bookings", "Weight");
            DropColumn("dbo.Bookings", "HeartRate");
        }
    }
}
