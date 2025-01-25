using Logic.Domain.Pieces;
using Logic.Domain.Players;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logic.Domain.BoardUtil
{
    public class Board
    {
        private readonly Piece?[,] _pieces = new Piece[8, 8];

        private readonly Dictionary<PlayerColor, Position?> _pawnSkipPositions = new Dictionary<PlayerColor, Position?>()
        {
            { PlayerColor.White, null },
            { PlayerColor.Black, null }
        };

        public Piece? this[int col, int row]
        {
            get { return _pieces[col, row]; }
            set { _pieces[col, row] = value; }
        }

        public Piece? this[Position position]
        {
            get { return _pieces[position.Col, position.Row]; }
            set { _pieces[position.Col, position.Row] = value; }
        }

        public Position? GetPawnSkipPosition(PlayerColor color)
        {
            return _pawnSkipPositions[color];
        }

        public void SetPawnSkipPosition(PlayerColor color, Position? position)
        {
            _pawnSkipPositions[color] = position;
        }

        public Board()
        {
            AddStartPieces();
        }

        private void AddStartPieces()
        {
            this[4, 3] = new Queen(PlayerColor.White);

            // AddNonPawnPiecesForColor(PlayerColor.White);
            // AddNonPawnPiecesForColor(PlayerColor.Black);
            // AddNonPawnPieces();
        }

        private void AddNonPawnPiecesForColor(PlayerColor color)
        {
            int row = color == PlayerColor.White ? 0 : 7;
            this[0, row] = new Rook(color);
            this[1, row] = new Knight(color);
            this[2, row] = new Bishop(color);
            this[3, row] = new Queen(color);
            this[4, row] = new King(color);
            this[5, row] = new Bishop(color);
            this[6, row] = new Knight(color);
            this[7, row] = new Rook(color);
        }

        private void AddNonPawnPieces()
        {
            for (int col = 0; col < 8; col++)
            {
                this[col, 1] = new Pawn(PlayerColor.White);
                this[col, 6] = new Pawn(PlayerColor.Black);
            }
        }

        public string ToSmallFen() // smallfen is only the pieces on the board without the next player and castling rights
        {
            StringBuilder fen = new StringBuilder(72);
            for (int row = 7; row >= 0; row--)
            {
                int emptyCount = 0;
                for (int col = 0; col < 8; col++)
                {
                    Piece? piece = this[col, row];
                    if (piece == null)
                    {
                        emptyCount++;
                        continue;
                    }
                    
                    if (emptyCount > 0)
                    {
                        fen.Append(emptyCount);
                        emptyCount = 0;
                    }
                    fen.Append(piece.ToFen());
                }

                if (emptyCount > 0)
                {
                    fen.Append(emptyCount);
                }

                if (row > 0)
                {
                    fen.Append('/');
                }
            }
            return fen.ToString();
        }

        public bool Contains(Position pos)
        {
            return pos.Col >= 0 && pos.Col < 8 && pos.Row >= 0 && pos.Row < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Position pos = new Position(col, row);
                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(PlayerColor color)
        {
            return PiecePositions().Where(pos => this[pos]?.Color == color);
        }

        public bool IsInCheck(PlayerColor color)
        {
            return PiecePositionsFor(color.GetOpponent()).Any(pos => {
                Piece piece = this[pos]!;
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new();

            foreach(Position pos in PiecePositions())
            {
                copy[pos] = this[pos]!.Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach(Position pos in PiecePositions())
            {
                Piece? piece = this[pos];
                if (piece != null) counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVsKing(counting) ||
                IsKingBishopVsKing(counting) ||
                IsKingKnightVsKing(counting) ||
                IsKingBishopVsKingBishop(counting);
        }

        private static bool IsKingVsKing(Counting counting)
        {
            return counting.TotalCount == 2; // must be kings then
        }

        private static bool IsKingBishopVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private bool IsKingBishopVsKingBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            Position wBishopPos = FindPiece(PlayerColor.White, PieceType.Bishop);
            Position bBishopPos = FindPiece(PlayerColor.Black, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private Position FindPiece(PlayerColor color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos]?.Type == type);
        }

        public override string ToString()
        {
            return ToSmallFen();
        }
    }
}
