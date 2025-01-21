using Logic.Domain.BoardUtil;
using Logic.Domain.Exceptions;
using Logic.Domain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Moves
{
    public class BasicMove : Move
    {
        public override MoveType Type => MoveType.Basic;
        public override Position Start { get; init; }
        public override Position End { get; init; }

        public BasicMove(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        public override void Execute(Board board)
        {
            Piece piece = board[Start] ?? throw new IllegalStateException("No piece at start location to move");
            board[End] = piece;
            board[Start] = null;
            piece.HasMoved = true;
        }
    }
}
