namespace TrashCollector2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrentDayToPickUpDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PickUpDays", "CurrentDay", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PickUpDays", "CurrentDay");
        }
    }
}
