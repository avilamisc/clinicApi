namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class AddGeolocationTableForClinic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clinics", "Geolocation", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clinics", "Geolocation");
        }
    }
}
