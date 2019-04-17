namespace Clinic.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddStageColumnForBookingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Stage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Stage");
        }
    }
}
