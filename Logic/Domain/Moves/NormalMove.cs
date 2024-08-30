using Logic.Domain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Moves
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Basic;
        public override Position Start { get; init; }
        public override Position End { get; init; }

        public NormalMove(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        public override void Execute(Board board)
        {
            Piece piece = board[Start];
            board[End] = piece;
            board[Start] = null;
            piece.HasMoved = true;
        }
    }
}
