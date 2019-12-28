namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class watchlistusertostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Watchlists", "userId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Watchlists", "userId", c => c.Int(nullable: false));
        }
    }
}
