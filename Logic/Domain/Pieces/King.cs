using Logic.Domain.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override PlayerColor Color { get; init; }
        private static readonly Direction[] _directions = new Direction[]
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.NorthWest
        };

        public King(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position startPosition, Board board)
        {
            foreach (Direction dir in _directions)
            {
                Position endPosition = startPosition + dir;

                if (!board.Contains(endPosition))
                {
                    continue;
                }

                if (board.IsEmpty(endPosition) || board[endPosition].Color != Color)
                {
                    yield return endPosition;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position startPosition, Board board)
        {
            // TODO: castling later
            foreach (Position endPosition in MovePositions(startPosition, board))
            {
                yield return new NormalMove(startPosition, endPosition);
            }
        }
    }
}
