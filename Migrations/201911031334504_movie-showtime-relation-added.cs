namespace Movie_Recommendation_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movieshowtimerelationadded : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CinemaShowtimes", name: "MoviesInstance_Id", newName: "MoviesId");
            RenameIndex(table: "dbo.CinemaShowtimes", name: "IX_MoviesInstance_Id", newName: "IX_MoviesId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.CinemaShowtimes", name: "IX_MoviesId", newName: "IX_MoviesInstance_Id");
            RenameColumn(table: "dbo.CinemaShowtimes", name: "MoviesId", newName: "MoviesInstance_Id");
        }
    }
}
