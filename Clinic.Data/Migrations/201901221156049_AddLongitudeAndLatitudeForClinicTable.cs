namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLongitudeAndLatitudeForClinicTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clinics", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Clinics", "Latitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clinics", "Latitude");
            DropColumn("dbo.Clinics", "Longitude");
        }
    }
}
