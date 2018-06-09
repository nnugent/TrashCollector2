namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInfoAndPickUpDaysToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PickUpDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        PhoneNumber = c.String(),
                        PickUpDayId = c.Int(),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickUpDays", t => t.PickUpDayId)
                .Index(t => t.PickUpDayId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "PickUpDayId", "dbo.PickUpDays");
            DropIndex("dbo.UserInfoes", new[] { "PickUpDayId" });
            DropTable("dbo.UserInfoes");
            DropTable("dbo.PickUpDays");
        }
    }
}
