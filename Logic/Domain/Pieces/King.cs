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
    }
}
