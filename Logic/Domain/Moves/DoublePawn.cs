using System;
using Logic.Domain.BoardUtil;
using Logic.Domain.Players;

namespace Logic.Domain.Moves;

public class DoublePawn : Move
{
    public override MoveType Type => MoveType.DoublePawn;
    public override Position Start { get; init; }
    public override Position End { get; init; }

    private readonly Position _skippedPos;

    public DoublePawn(Position start, Position end)
    {
        Start = start;
        End = end;
        _skippedPos = new Position(start.Col, (start.Row + end.Row) / 2);
    }

    public override void Execute(Board board)
    {
        PlayerColor color = board[Start]!.Color;
        board.SetPawnSkipPosition(color, _skippedPos);
        new BasicMove(Start, End).Execute(board);
    }
}
