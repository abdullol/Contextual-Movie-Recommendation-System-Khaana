namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cinemaShowtimelocationAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CinemaShowtimes", "Location", c => c.String());
            DropColumn("dbo.Cinemas", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cinemas", "Location", c => c.String());
            DropColumn("dbo.CinemaShowtimes", "Location");
        }
    }
}
