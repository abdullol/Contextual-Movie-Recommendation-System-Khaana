namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cinemalocationadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cinemas", "Location", c => c.String());
            DropColumn("dbo.Cinemas", "StaticLocation");
            DropColumn("dbo.Cinemas", "latitude");
            DropColumn("dbo.Cinemas", "longitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cinemas", "longitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Cinemas", "latitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Cinemas", "StaticLocation", c => c.String());
            DropColumn("dbo.Cinemas", "Location");
        }
    }
}
