using Logic.Domain;
using Logic.Domain.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Service
{
    public interface IChessApiService
    {
        IEnumerable<ChessGame> GetGames();
        ChessGame GetGame(string gameId);
        ChessGame CreateGame(string username);
        ChessGame JoinGame(string gameId, string username);
        IEnumerable<Move> GetValidMoves(string gameId, string startPositionString);
    }
}
