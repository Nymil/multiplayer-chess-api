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
    }
}
