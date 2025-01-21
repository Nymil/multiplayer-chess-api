using System.Diagnostics;
using AutoMapper;
using Logic.Domain;
using Logic.Domain.Exceptions;
using Logic.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MultiplayerChessApi.Response;

namespace MultiplayerChessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IChessApiService _service;
        private readonly IMapper _mapper;

        public GamesController(IChessApiService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public OkObjectResult GetGames()
        {
            IEnumerable<ChessGame> games = _service.GetGames();
            return Ok(games.Select(_mapper.Map<AllGamesResponse>));
        }

        [HttpGet("{id}")]
        public OkObjectResult GetGame([FromRoute] string id)
        {
            ChessGame game = _service.GetGame(id) ?? throw new ChessNotFoundException($"No game with id {id}");
            GameByIdResponse response = _mapper.Map<GameByIdResponse>(game);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateGame()
        {
            ChessGame newGame = _service.CreateGame();
            GameCreatedResponse response = _mapper.Map<GameCreatedResponse>(newGame);
            return CreatedAtAction(nameof(GetGame), new { id = response.GameId }, response);
        }
    }
}
