using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public enum PieceType
    {
        Pawn,
        Bishop,
        Knight,
        Rook,
        Queen,
        King
    }

    public static class PieceTypeExtensions
    {
        public static char GetFenLetter(this PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Pawn => 'p',
                PieceType.Bishop => 'b',
                PieceType.Knight => 'n',
                PieceType.Rook => 'r',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, "Invalid piece type."),
            };
        }
    }
}
