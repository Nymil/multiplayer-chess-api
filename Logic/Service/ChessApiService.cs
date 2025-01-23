using Logic.Domain;
using Logic.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Service
{
    public class ChessApiService : IChessApiService
    {
        private static readonly Lazy<ChessApiService> _instance = new Lazy<ChessApiService>(() => new ChessApiService());
        private ICollection<ChessGame> _games = new List<ChessGame>();
        public static ChessApiService Instance => _instance.Value;
        private ChessApiService() {}

        public IEnumerable<ChessGame> GetGames()
        {
            return _games;
        }

        public ChessGame? GetGame(string id)
        {
            return _games.FirstOrDefault(game => game.Id == id);
        }

        public ChessGame CreateGame(string username)
        {
            ChessGame newGame = new ChessGame(username);
            _games.Add(newGame);
            return newGame;
        }

        public ChessGame JoinGame(string gameId, string username)
        {
            ChessGame game = GetGame(gameId) ?? throw new ChessNotFoundException($"No game with id {gameId}");
            game.JoinGame(username);
            return game;
        }
    }
}
