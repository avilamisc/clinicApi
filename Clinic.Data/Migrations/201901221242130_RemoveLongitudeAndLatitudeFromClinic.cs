namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLongitudeAndLatitudeFromClinic : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clinics", "Longitude");
            DropColumn("dbo.Clinics", "Latitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clinics", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Clinics", "Longitude", c => c.Double(nullable: false));
        }
    }
}
