using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Domain.BoardUtil;

namespace Logic.Domain.Moves
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position Start { get; init; }
        public abstract Position End { get; init; }
        public abstract void Execute(Board board);
        public override bool Equals(object? obj)
        {
            if (obj is Move otherMove)
            {
                return Start.Equals(otherMove.Start)
                    && End.Equals(otherMove.End);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Start,
                End
            );
        }
    }
}
