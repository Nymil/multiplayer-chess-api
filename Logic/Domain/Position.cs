using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain
{
    public class Position
    {
        public int Row { get; init; }
        public int Col { get; init; }

        public Position(int col, int row)
        {
            Col = col;
            Row = row;
        }

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Col + dir.DeltaCol, pos.Row + dir.DeltaRow);
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Col == position.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(Position? pos1, Position? pos2)
        {
            return EqualityComparer<Position>.Default.Equals(pos1, pos2);
        }

        public static bool operator !=(Position? pos1, Position? pos2)
        {
            return !(pos1 == pos2);
        }
    }
}
