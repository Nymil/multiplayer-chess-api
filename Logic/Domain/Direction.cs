using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain
{
    public class Direction
    {
        public static Direction North => new Direction(0, 1);
        public static Direction South => new Direction(0, -1);
        public static Direction East => new Direction(1, 0);
        public static Direction West => new Direction(-1, 0);
        public static Direction NorthEast => North + East;
        public static Direction NorthWest => North + West;
        public static Direction SouthEast => South + East;
        public static Direction SouthWest => South + West;

        public int DeltaCol { get; init; }
        public int DeltaRow { get; init; }

        public Direction(int deltaCol, int deltaRow)
        {
            DeltaCol = deltaCol;
            DeltaRow = deltaRow;
        }

        public static Direction operator +(Direction a, Direction b)
        {
            return new Direction(a.DeltaCol + b.DeltaCol, a.DeltaRow + b.DeltaRow);
        }

        public static Direction operator *(int scalar, Direction direction)
        {
            return new Direction(scalar * direction.DeltaCol, scalar * direction.DeltaRow);
        }
    }
}
