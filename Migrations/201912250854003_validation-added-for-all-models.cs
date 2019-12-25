namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validationaddedforallmodels : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cinemas", "CinemaName", c => c.String(nullable: false));
            AlterColumn("dbo.CinemaShowtimes", "ShowTime", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "MovieName", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "MoviePoster", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "MovieRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Movies", "PGRating", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "MovieDuration", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "ReleaseDate", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Director", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Actor", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "MovieGenre", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Story", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Story", c => c.String());
            AlterColumn("dbo.Movies", "MovieGenre", c => c.String());
            AlterColumn("dbo.Movies", "Actor", c => c.String());
            AlterColumn("dbo.Movies", "Director", c => c.String());
            AlterColumn("dbo.Movies", "ReleaseDate", c => c.String());
            AlterColumn("dbo.Movies", "MovieDuration", c => c.String());
            AlterColumn("dbo.Movies", "PGRating", c => c.String());
            AlterColumn("dbo.Movies", "MovieRating", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Movies", "MoviePoster", c => c.String());
            AlterColumn("dbo.Movies", "MovieName", c => c.String());
            AlterColumn("dbo.CinemaShowtimes", "ShowTime", c => c.String());
            AlterColumn("dbo.Cinemas", "CinemaName", c => c.String());
        }
    }
}
