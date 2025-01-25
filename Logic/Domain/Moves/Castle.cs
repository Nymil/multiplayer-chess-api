using System;
using System.Runtime.CompilerServices;
using Logic.Domain.BoardUtil;
using Logic.Domain.Players;

namespace Logic.Domain.Moves;

public class Castle : Move
{
    public override MoveType Type { get; }
    public override Position Start { get; init; }
    public override Position End { get; init; }

    private readonly Direction KingMoveDir;
    private readonly Position rookStartPos;
    private readonly Position rookEndPos;

    public Castle(MoveType type, Position kingPos)
    {
        Type = type;
        Start = kingPos;

        if (type == MoveType.CastleKS)
        {
            KingMoveDir = Direction.East;
            End = new Position(6, kingPos.Row);
            rookStartPos = new Position(7, kingPos.Row);
            rookEndPos = new Position(5, kingPos.Row);
        }
        else if (type == MoveType.CasltQS)
        {
            KingMoveDir = Direction.West;
            End = new Position(2, kingPos.Row);
            rookStartPos = new Position(0, kingPos.Row);
            rookEndPos = new Position(3, kingPos.Row);
        }
        else
        {
            throw new ArgumentException("Invalid castle type");
        }
    }

    public override bool Execute(Board board)
    {
        new BasicMove(Start, End).Execute(board);
        new BasicMove(rookStartPos, rookEndPos).Execute(board);

        return false;
    }

    public override bool IsLegal(Board board)
    {
        PlayerColor color = board[Start]!.Color;

        if (board.IsInCheck(color))
        {
            return false;
        }

        Board copy = board.Copy();
        Position kingPosInCopy = Start;

        for (int i = 0; i < 2; i++)
        {
            new BasicMove(kingPosInCopy, kingPosInCopy + KingMoveDir).Execute(copy);
            kingPosInCopy += KingMoveDir;

            if (copy.IsInCheck(color))
            {
                return false;
            }
        }

        return true;
    }
}
