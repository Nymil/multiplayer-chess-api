using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Logic.Domain.BoardUtil;
using Logic.Domain.Exceptions;

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
            return obj is Move otherMove &&
                   Start == otherMove.Start &&
                   End == otherMove.End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start,End);
        }

        public override string ToString()
        {
            return $"{Start}{End}";
        }

        public static bool operator ==(Move? move1, Move? move2)
        {
            return EqualityComparer<Move>.Default.Equals(move1, move2);
        }

        public static bool operator !=(Move? move1, Move? move2)
        {
            return !(move1 == move2);
        }

        public static void ValidateMoveString(string moveString)
        {
            if (moveString.Length != 4)
            {
                throw new ChessBadRequestException("Move string must be four characters long");
            }

            // testing if the first and second characters are valid positions
            // constructor woudl throw error if invalid
            _ = new Position(moveString[..2]);
            _ = new Position(moveString.Substring(2, 2));
        }
    }
}
