using Logic.Domain.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override PlayerColor Color { get; init; }

        public Knight(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> PotentialEndPositions(Position startPosition)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South } )
            {
                foreach (Direction hDir in new Direction[] { Direction.East, Direction.West })
                {
                    yield return startPosition + 2 * vDir + hDir;
                    yield return startPosition + 2 * hDir + vDir;
                }
            }
        }

        private bool CanMoveOrCapture(Position position, Board board)
        {
            if (!board.Contains(position))
            {
                return false;
            }
            return board.IsEmpty(position) || board[position].Color != Color;
        }

        private IEnumerable<Position> MovePositions(Position startPosition, Board board)
        {
            return PotentialEndPositions(startPosition).Where(pos => CanMoveOrCapture(startPosition, board));
        }

        public override IEnumerable<Move> GetMoves(Position startPosition, Board board)
        {
            return MovePositions(startPosition, board).Select(endPosition => new NormalMove(startPosition, endPosition));
        }
    }
}
