namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePatientLocationColumnAddBornDateColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "BornDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Patients", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patients", "Location", c => c.String());
            DropColumn("dbo.Patients", "BornDate");
        }
    }
}
