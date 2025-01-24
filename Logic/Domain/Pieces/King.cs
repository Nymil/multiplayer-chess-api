using Logic.Domain.BoardUtil;
using Logic.Domain.Moves;
using Logic.Domain.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override PlayerColor Color { get; init; }
        private static readonly Direction[] _directions = new Direction[]
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.NorthWest
        };

        public King(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private bool CanCaptureAt(Position position, Board board)
        {
            if (!board.Contains(position) || board.IsEmpty(position))
            {
                return false;
            }

            Piece piece = board[position]!;
            return piece.Color != Color;
        }

        private IEnumerable<Position> MovePositions(Position startPosition, Board board)
        {
            foreach (Direction dir in _directions)
            {
                Position endPosition = startPosition + dir;

                if (!board.Contains(endPosition))
                {
                    continue;
                }

                if (board.IsEmpty(endPosition) || CanCaptureAt(endPosition, board))
                {
                    yield return endPosition;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position startPosition, Board board)
        {
            // TODO: castling later
            foreach (Position endPosition in MovePositions(startPosition, board))
            {
                yield return new BasicMove(startPosition, endPosition);
            }
        }

        // override because castling can not capture an opponent king
        public override bool CanCaptureOpponentKing(Position startPosition, Board board)
        {
            return MovePositions(startPosition, board).Any(endPosition => {
                Piece? piece = board[endPosition];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
