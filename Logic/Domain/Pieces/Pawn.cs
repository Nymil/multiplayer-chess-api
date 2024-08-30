using Logic.Domain.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Pieces
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override PlayerColor Color { get; init; }
        private readonly Direction _forward;

        public Pawn(PlayerColor color)
        {
            Color = color;

            if (color == PlayerColor.White)
            {
                _forward = Direction.North;
            }
            else
            {
                _forward = Direction.South;
            }
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private bool CanMoveTo(Position position, Board board)
        {
            return board.IsEmpty(position) && board.Contains(position);
        }

        private bool CanCaptureAt(Position position, Board board)
        {
            if (board.IsEmpty(position) || !board.Contains(position))
            {
                return false;
            }

            Piece piece = board[position];
            return piece.Color != Color;
        }

        private IEnumerable<Move> ForwardMoves(Position startPosition, Board board  )
        {
            Position forwardPosition = startPosition + _forward;
            if (CanMoveTo(forwardPosition, board))
            {
                yield return new NormalMove(startPosition, forwardPosition);

                Position doubleForwardPosition = forwardPosition + _forward;
                if (!HasMoved && CanMoveTo(doubleForwardPosition, board))
                {
                    yield return new NormalMove(startPosition, doubleForwardPosition);
                }
            }
        }

        private IEnumerable<Move> CaptureMoves(Position startPosition, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                Position endPosition = startPosition + _forward + dir;

                if (CanCaptureAt(endPosition, board))
                {
                    yield return new NormalMove(startPosition, endPosition);
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position startPosition, Board board)
        {
            return ForwardMoves(startPosition, board).Concat(CaptureMoves(startPosition, board));
        }
    }
}
