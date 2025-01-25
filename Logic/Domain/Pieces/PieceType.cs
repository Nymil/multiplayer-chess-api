using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Domain.Exceptions;

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

        public static PieceType PromotionPieceFromString(string pieceName)
        {
            return pieceName.ToLower() switch
            {
                "queen" => PieceType.Queen,
                "rook" => PieceType.Rook,
                "bishop" => PieceType.Bishop,
                "knight" => PieceType.Knight,
                _ => throw new ChessBadRequestException($"Invalid promotion piece name {pieceName}")
            };
        }
    }
}
