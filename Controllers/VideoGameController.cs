using apiWebpractice.Models;
using apiWebpractice.VideoGameDBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiWebpractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VideoGameController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<List<VideoGame>>> GetAllVideoGames()
        {
            if(_context.videoGames==null)
            {
                return NotFound();
            }
           
            return Ok(await _context.videoGames.ToListAsync());
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {
            var videoGame = await _context.videoGames.FindAsync(id);
            if (videoGame == null)
            {
                return NotFound();
            }
            return Ok(videoGame);
        }

        [HttpPost]
        public async Task<ActionResult<VideoGame>> CreateVideoGame(VideoGame videoGame)
        {
            if (videoGame == null)
            {
                return BadRequest();
            }
            _context.videoGames.Add(videoGame);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVideoGameById), new { id = videoGame.Id }, videoGame);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VideoGame>> UpdateVideoGame(int id, VideoGame videoGame)
        {
            var game = await _context.videoGames.FindAsync(id);
            if (videoGame == null)
            {
                return BadRequest();

            }

            game.Name = videoGame.Name;
            game.Genre = videoGame.Genre;
            game.ReleaseYear = videoGame.ReleaseYear;
            await _context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<VideoGame>> DeleteVideoGame(int id)
        {
            var videoGame = await _context.videoGames.FindAsync(id);
            if (videoGame == null)
            {
                return NotFound();
            }
            _context.videoGames.Remove(videoGame);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
