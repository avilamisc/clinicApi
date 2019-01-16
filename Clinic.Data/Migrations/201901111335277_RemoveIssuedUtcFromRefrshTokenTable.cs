namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIssuedUtcFromRefrshTokenTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RefreshTokens", "IssuedUtc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RefreshTokens", "IssuedUtc", c => c.DateTime(nullable: false));
        }
    }
}
