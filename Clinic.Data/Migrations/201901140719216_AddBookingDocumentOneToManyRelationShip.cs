namespace Clinic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingDocumentOneToManyRelationShip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "BookingId", c => c.Int());
            CreateIndex("dbo.Documents", "BookingId");
            AddForeignKey("dbo.Documents", "BookingId", "dbo.Bookings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "BookingId", "dbo.Bookings");
            DropIndex("dbo.Documents", new[] { "BookingId" });
            DropColumn("dbo.Documents", "BookingId");
        }
    }
}
