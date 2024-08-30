using Logic.Domain.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override PlayerColor Color { get; init; }
        private static readonly Direction[] _directions = new Direction[]
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West
        };

        public Rook(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Rook copy = new Rook(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position startPosition, Board board)
        {
            return MovePositionsOfDirection(startPosition, board, _directions)
                .Select(endPosition => new NormalMove(startPosition, endPosition));
        }
    }
}
