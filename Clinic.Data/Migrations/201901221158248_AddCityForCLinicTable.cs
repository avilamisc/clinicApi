namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCityForCLinicTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clinics", "City", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clinics", "City");
        }
    }
}
