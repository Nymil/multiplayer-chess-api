using Logic.Domain.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;
        public override PlayerColor Color { get; init; }
        private static readonly Direction[] _directions = new Direction[] 
        { 
            Direction.NorthEast, 
            Direction.NorthWest, 
            Direction.SouthEast, 
            Direction.SouthWest
        };

        public Bishop(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Bishop copy = new Bishop(Color);
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
