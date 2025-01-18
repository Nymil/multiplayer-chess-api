using Logic.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiplayerChessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private static IChessApiService _service = ChessApiService.Instance;

        [HttpGet]
        public IActionResult GetGames()
        {
            return Ok("Getting all games");
        }
    }
}
