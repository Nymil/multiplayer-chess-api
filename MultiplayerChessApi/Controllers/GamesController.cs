using System.Diagnostics;
using AutoMapper;
using Logic.Domain;
using Logic.Domain.Exceptions;
using Logic.Domain.Moves;
using Logic.Service;
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
            IEnumerable<ChessGame> games = _service.GetGames();
            return Ok(games.Select(_mapper.Map<AllGamesResponse>));
        }

        [HttpPatch("join/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameJoinedResponse))]
        public IActionResult JoinGame([FromRoute] string id, [FromBody] JoinGameRequest request)
        {            
            ValidateJoinGameRequest(request);
            string username = request.Username!.Trim();
            ChessGame game = _service.JoinGame(id, username);
            string userUuid = game.PlayerBlack!.Uuid;

            GameJoinedResponse response = _mapper.Map<GameJoinedResponse>(_service.GetGame(id));
            SetCookies(userUuid);
            return Ok(response);
        }

        [HttpGet("{id}/valid-moves")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ValidMovesResponse))]
        public IActionResult GetValidMoves([FromRoute] string id, [FromQuery] string position)
        {
            ChessGame game = _service.GetGame(id);
            ValidateCanViewContent(game);

            IEnumerable<Move> validMoves = _service.GetValidMoves(id, position);
            ValidMovesResponse response = new ValidMovesResponse { // mapper doesn't work starting from collections
                ValidMoves = validMoves.Select(move => move.ToString()).ToArray()
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameByIdResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public IActionResult GetGame([FromRoute] string id)
        {
            ChessGame game = _service.GetGame(id);
            ValidateCanViewContent(game);

            GameByIdResponse response = _mapper.Map<GameByIdResponse>(game);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GameCreatedResponse))]
        // todo: add other possible error response types
        public IActionResult CreateGame([FromBody] CreateGameRequest request)
        {
            ValidateCreateGameRequest(request);
            string username = request.Username!.Trim();

            ChessGame newGame = _service.CreateGame(username);
            string userUuid = newGame.PlayerWhite.Uuid;

            GameCreatedResponse response = _mapper.Map<GameCreatedResponse>(newGame);
            SetCookies(userUuid);
            return CreatedAtAction(nameof(GetGame), new { id = response.GameId }, response);
        }

        [HttpPatch("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameByIdResponse))]
        public IActionResult ExecuteMove([FromRoute] string id, [FromBody] ExecuteMoveRequest request)
        {
            ValidateExecuteMoveRequest(request);

            ChessGame game = _service.GetGame(id);
            ValidateCanExecuteMove(game);

            _service.ExecuteMove(id, request.Move!);

            GameByIdResponse response = _mapper.Map<GameByIdResponse>(_service.GetGame(id));
            return Ok(response);
        }

        private void SetCookies(string userUuid)
        {
            Response.Cookies.Append("UserUUID", userUuid, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            });
        }

        private void ValidateCanViewContent(ChessGame game)
        {
            string requestUserUuid = Request.Cookies["UserUUID"] ?? throw new ChessUnauthorizedException("You are not authorized to view this game");

            if (!CanViewContent(game, requestUserUuid))
            {
                throw new ChessForbidenException($"You are forbidden to view this game with uuid {requestUserUuid}");
            }
        }

        private void ValidateCanExecuteMove(ChessGame game)
        {
            ValidateCanViewContent(game);
            string requestUserUuid = Request.Cookies["UserUUID"]!;
            if (game.CurrentPlayer.Uuid != requestUserUuid)
            {
                throw new ChessForbidenException("It's not your turn");
            }
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

        private void ValidateExecuteMoveRequest(ExecuteMoveRequest request)
        {
            if (request.Move == null || string.IsNullOrWhiteSpace(request.Move))
            {
                throw new ChessBadRequestException("Move is required");
            }
        }
    }
}
