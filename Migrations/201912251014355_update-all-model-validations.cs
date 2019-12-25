namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateallmodelvalidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CinemaShowtimes", "Location", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CinemaShowtimes", "Location", c => c.String());
        }
    }
}
