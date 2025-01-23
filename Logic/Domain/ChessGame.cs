using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Domain.BoardUtil;
using Logic.Domain.Exceptions;
using Logic.Domain.Moves;
using Logic.Domain.Pieces;
using Logic.Domain.Players;

namespace Logic.Domain
{
    public class ChessGame
    {
    
        public string Id { get; init; }
        public Board Board { get; init; }
        public PlayerColor CurrentPlayer { get; private set;} = PlayerColor.White;
        public ChessGameState State { get; private set; } = ChessGameState.Waiting;


        public ChessGame()
        {
            Id = Guid.NewGuid().ToString();
            Board = new Board();
        }

        public IEnumerable<Move> LegalMovesForPiece(Position startPosition)
        {
            if (Board.IsEmpty(startPosition) || Board[startPosition]?.Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[startPosition]!;
            return piece.GetMoves(startPosition, Board);
        }

        public void MakeMove(Move move)
        {
            ValidateMove(move);
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.GetOpponent();
        }

        private void ValidateMove(Move move)
        {
            IEnumerable<Move> legalMoves = LegalMovesForPiece(move.Start);
            if (!legalMoves.Contains(move))
            {
                throw new ChessIllegalStateException("Illegal move");
            }

            if (State != ChessGameState.InProgress)
            {
                throw new ChessIllegalStateException("Game is not in progress");
            }
        }
    }
}
