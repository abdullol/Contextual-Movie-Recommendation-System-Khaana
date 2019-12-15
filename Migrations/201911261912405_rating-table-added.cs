namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingtableadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        movieId = c.Int(),
                        rating = c.Int(nullable: false),
                        timestamp = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.movieId)
                .Index(t => t.movieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "movieId", "dbo.Movies");
            DropIndex("dbo.Ratings", new[] { "movieId" });
            DropTable("dbo.Ratings");
        }
    }
}
