namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPickedUpAndExtraPickUpDayToUserInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfoes", "PickedUp", c => c.Int(nullable: false));
            AddColumn("dbo.UserInfoes", "ExtraPickUpId", c => c.Int());
            CreateIndex("dbo.UserInfoes", "ExtraPickUpId");
            AddForeignKey("dbo.UserInfoes", "ExtraPickUpId", "dbo.PickUpDays", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "ExtraPickUpId", "dbo.PickUpDays");
            DropIndex("dbo.UserInfoes", new[] { "ExtraPickUpId" });
            DropColumn("dbo.UserInfoes", "ExtraPickUpId");
            DropColumn("dbo.UserInfoes", "PickedUp");
        }
    }
}
