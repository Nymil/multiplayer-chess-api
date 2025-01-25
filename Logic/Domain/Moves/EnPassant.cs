using System;
using Logic.Domain.BoardUtil;

namespace Logic.Domain.Moves;

public class EnPassant : Move
{
    public override MoveType Type => MoveType.EnPassant;
    public override Position Start { get; init; }
    public override Position End { get; init; }

    private readonly Position _capturePos;

    public EnPassant(Position start, Position end)
    {
        Start = start;
        End = end;
        _capturePos = new Position(end.Col, start.Row);
    }

    public override bool Execute(Board board)
    {
        new BasicMove(Start, End).Execute(board);
        board[_capturePos] = null;

        return true;
    }
}
