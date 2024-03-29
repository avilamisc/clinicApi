namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleColumnForUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Role");
        }
    }
}
