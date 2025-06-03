using System.Composition;

namespace apiWebpractice.Models
{
    public class VideoGame
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Genre { get; set; }

        public int? ReleaseYear { get; set; }
    }
}
