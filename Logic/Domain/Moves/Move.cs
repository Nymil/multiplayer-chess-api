using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Moves
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position Start { get; init; }
        public abstract Position End { get; init; }
        public abstract void Execute(Board board);
    }
}
