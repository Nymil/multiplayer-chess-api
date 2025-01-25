using Logic.Domain;
using Logic.Domain.BoardUtil;
using Logic.Domain.Exceptions;
using Logic.Domain.Moves;
using Logic.Domain.Pieces;
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

        public ChessGame GetGame(string id)
        {
            return _games.FirstOrDefault(game => game.Id == id) ?? throw new ChessNotFoundException($"No game with id {id}");
        }

        public ChessGame CreateGame(string username)
        {
            ChessGame newGame = new ChessGame(username);
            _games.Add(newGame);
            return newGame;
        }

        public ChessGame JoinGame(string gameId, string username)
        {
            ChessGame game = GetGame(gameId);
            game.JoinGame(username);
            return game;
        }

        public ChessGame ExecuteMove(string gameId, string moveString, string? promotionPiece)
        {
            ChessGame game = GetGame(gameId);

            Move.ValidateMoveString(moveString);
            string startPositionString = moveString[..2];
            Position startPosition = new(startPositionString);

            IEnumerable<Move> legalMoves = game.LegalMovesForPiece(startPosition);
            Move move = legalMoves.FirstOrDefault(m => m.ToString() == moveString)
                ?? throw new ChessBadRequestException("Illegal move");

            if (move is PawnPromotion)
            {
                if (promotionPiece == null)
                {
                    throw new ChessBadRequestException("Promotion piece must be specified");
                }

                PieceType promotionType = PieceTypeExtensions.PromotionPieceFromString(promotionPiece);
                move = legalMoves.FirstOrDefault(m => m is PawnPromotion pp && pp.NewType == promotionType)
                    ?? throw new ChessBadRequestException("Invalid promotion piece");
            }

            game.MakeMove(move);
            return game;
        }

        public IEnumerable<Move> GetValidMoves(string gameId, string startPositionString)
        {
            ChessGame game = GetGame(gameId);
            Position startPosition = new Position(startPositionString);
            return game.LegalMovesForPiece(startPosition);
        }
    }
}
