namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLatitudeAndLongitudeToUserInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfoes", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.UserInfoes", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfoes", "Longitude");
            DropColumn("dbo.UserInfoes", "Latitude");
        }
    }
}
