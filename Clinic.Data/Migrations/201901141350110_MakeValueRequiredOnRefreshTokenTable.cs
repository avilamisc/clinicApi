namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeValueRequiredOnRefreshTokenTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RefreshTokens", "Value", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RefreshTokens", "Value", c => c.String());
        }
    }
}
