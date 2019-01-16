namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilePathRemoveContentsFieldsInDocumentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "FilePath", c => c.String());
            DropColumn("dbo.Documents", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Content", c => c.String());
            DropColumn("dbo.Documents", "FilePath");
        }
    }
}
