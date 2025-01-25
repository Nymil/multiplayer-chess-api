using System;
using Logic.Domain.BoardUtil;
using Logic.Domain.Pieces;
using Logic.Domain.Players;

namespace Logic.Domain.Moves;

public class PawnPromotion : Move
{
    public override MoveType Type => MoveType.Promotion;
    public override Position Start { get; init; }
    public override Position End { get; init; }

    private readonly PieceType _newType;
    public PieceType NewType => _newType;

    public PawnPromotion(Position start, Position end, PieceType newType)
    {
        Start = start;
        End = end;
        _newType = newType;
    }

    private Piece CreatePromotionPiece(PlayerColor color)
    {
        return _newType switch
        {
            PieceType.Queen => new Queen(color),
            PieceType.Rook => new Rook(color),
            PieceType.Bishop => new Bishop(color),
            PieceType.Knight => new Knight(color),
            _ => throw new ArgumentOutOfRangeException(nameof(_newType), _newType, null)
        };
    }

    public override bool Execute(Board board)
    {
        Piece pawn = board[Start]!;
        board[End] = null;

        Piece promotionPiece = CreatePromotionPiece(pawn.Color);
        promotionPiece.HasMoved = true;
        board[End] = promotionPiece;
        board[Start] = null;

        return true;
    }
}
