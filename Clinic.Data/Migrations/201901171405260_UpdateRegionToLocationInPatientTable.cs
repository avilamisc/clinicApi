namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRegionToLocationInPatientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "Location", c => c.String());
            DropColumn("dbo.Patients", "Region");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patients", "Region", c => c.String());
            DropColumn("dbo.Patients", "Location");
        }
    }
}
