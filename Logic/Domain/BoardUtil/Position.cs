using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Domain.Exceptions;

namespace Logic.Domain.BoardUtil
{
    public class Position
    {
        public int Row { get; init; }
        public int Col { get; init; }

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Col + dir.DeltaCol, pos.Row + dir.DeltaRow);
        }

        public Position(int col, int row)
        {
            Row = row;
            Col = col;
        }

        public Position(string positionString)
        {
            ValidatePositionString(positionString);

            Col = positionString[0] - 'a';
            Row = positionString[1] - '1';
        }

        private void ValidatePositionString(string positionString)
        {
            if (positionString.Length != 2)
            {
                throw new ChessBadRequestException("Position string must be two characters long");
            }

            char col = positionString[0];
            char row = positionString[1];

            if (col < 'a' || col > 'h' || row < '1' || row > '8')
            {
                throw new ChessBadRequestException("Position string must be within the range 'a1' to 'h8'");
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Col == position.Col;
        }

        public override string ToString()
        {
            char col = (char)('a' + Col);
            char row = (char)('1' + Row);
            return $"{col}{row}";
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
