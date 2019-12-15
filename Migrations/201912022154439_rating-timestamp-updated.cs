namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingtimestampupdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ratings", "timestamp", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ratings", "timestamp", c => c.String());
        }
    }
}
