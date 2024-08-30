using Logic.Domain.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract PlayerColor Color { get; init; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();
        public abstract IEnumerable<Move> GetMoves(Position startPosition, Board board);
        protected IEnumerable<Position> MovePositionsOfDirection(Position startPosition, Board board, Direction direction)
        {
            for (Position pos = startPosition + direction; board.Contains(pos); pos += direction)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }

                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;
                }

                yield break;
            }
        }

        protected IEnumerable<Position> MovePositionsOfDirection(Position startPosition, Board board, Direction[] directions)
        {
            return directions.SelectMany(dir => MovePositionsOfDirection(startPosition, board, dir));
        }
    }
}
