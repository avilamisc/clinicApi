namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCreationDateTypeToOffset : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "CreationDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "CreationDate", c => c.DateTime(nullable: false));
        }
    }
}
