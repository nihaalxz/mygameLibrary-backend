using apiWebpractice.Models;
using Microsoft.EntityFrameworkCore;

namespace apiWebpractice.VideoGameDBContext
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<VideoGame> videoGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VideoGame>().HasData(
                new VideoGame { Id = 1, Name = "The Legend of Zelda: Breath of the Wild", Genre = "Action-adventure", ReleaseYear = 2017 },
                new VideoGame { Id = 2, Name = "The Witcher 3: Wild Hunt", Genre = "Action role-playing", ReleaseYear = 2015 },
                new VideoGame { Id = 3, Name = "Dark Souls III", Genre = "Action role-playing", ReleaseYear = 2016 },
                new VideoGame { Id = 4, Name = "God of War", Genre = "Action-adventure", ReleaseYear = 2018 },
                new VideoGame { Id = 5, Name = "Red Dead Redemption 2", Genre = "Action-adventure", ReleaseYear = 2018 }
                );
        }
    }
}
