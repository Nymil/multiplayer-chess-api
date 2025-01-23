using System.Diagnostics;
using AutoMapper;
using Logic.Domain;
using Logic.Domain.Exceptions;
using Logic.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MultiplayerChessApi.Requests;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AllGamesResponse>))]
        public IActionResult GetGames()
        {
            ClearCookies();

            IEnumerable<ChessGame> games = _service.GetGames();
            return Ok(games.Select(_mapper.Map<AllGamesResponse>));
        }

        [HttpPatch("join/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameJoinedResponse))]
        public IActionResult JoinGame([FromRoute] string id, [FromBody] JoinGameRequest request)
        {
            ClearCookies();
            
            ValidateJoinGameRequest(request);
            string username = request.Username!.Trim();
            ChessGame game = _service.JoinGame(id, username);
            string userUuid = game.PlayerBlack!.Uuid;

            GameJoinedResponse response = _mapper.Map<GameJoinedResponse>(_service.GetGame(id));
            SetCookies(userUuid);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameByIdResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public IActionResult GetGame([FromRoute] string id)
        {
            ClearCookies();

            string requestUserUuid = Request.Cookies["UserUUID"] ?? throw new ChessUnauthorizedException("You are not authorized to view this game");
            ChessGame game = _service.GetGame(id) ?? throw new ChessNotFoundException($"No game with id {id}");

            if (!CanViewContent(game, requestUserUuid))
            {
                throw new ChessForbidenException($"You are forbidden to view this game with uuid {requestUserUuid}");
            }

            GameByIdResponse response = _mapper.Map<GameByIdResponse>(game);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GameCreatedResponse))]
        // todo: add other possible error response types
        public IActionResult CreateGame([FromBody] CreateGameRequest request)
        {
            ClearCookies();

            ValidateCreateGameRequest(request);
            string username = request.Username!.Trim();

            ChessGame newGame = _service.CreateGame(username);
            string userUuid = newGame.PlayerWhite.Uuid;

            // send response
            GameCreatedResponse response = _mapper.Map<GameCreatedResponse>(newGame);
            SetCookies(userUuid);
            return CreatedAtAction(nameof(GetGame), new { id = response.GameId }, response);
        }

        private void ClearCookies()
        {
            Response.Cookies.Delete("UserUUID");
        }

        private void SetCookies(string userUuid)
        {
            Response.Cookies.Append("UserUUID", userUuid, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });
        }

        private bool CanViewContent(ChessGame game, string requestUserUuid)
        {
            return game.PlayerWhite.Uuid == requestUserUuid || game.PlayerBlack?.Uuid == requestUserUuid;
        }

        private void ValidateCreateGameRequest(CreateGameRequest request)
        {
            if (request.Username == null || string.IsNullOrWhiteSpace(request.Username))
            {
                throw new ChessBadRequestException("Username is required");
            }
        }

        private void ValidateJoinGameRequest(JoinGameRequest request)
        {
            if (request.Username == null || string.IsNullOrWhiteSpace(request.Username))
            {
                throw new ChessBadRequestException("Username is required");
            }
        }
    }
}
