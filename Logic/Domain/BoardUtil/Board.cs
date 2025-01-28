using Logic.Domain.Moves;
using Logic.Domain.Pieces;
using Logic.Domain.Players;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

namespace Logic.Domain.BoardUtil
{
    public class Board
    {
        private readonly Piece?[,] _pieces = new Piece[8, 8];

        private readonly ICollection<Piece> _capturedPieces = new List<Piece>();
        public IEnumerable<Piece> CapturedPieces => _capturedPieces;

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
            AddNonPawnPiecesForColor(PlayerColor.White);
            AddNonPawnPiecesForColor(PlayerColor.Black);
            AddNonPawnPieces();
        }

        public void AddCapturedPiece(Piece piece)
        {
            _capturedPieces.Add(piece);
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
            copy.Clear();

            foreach(Position pos in PiecePositions())
            {
                copy[pos] = this[pos]!.Copy();
            }

            return copy;
        }

        private void Clear()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    this[col, row] = null;
                }
            }
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

        public string ToFen(PlayerColor currentPlayer)
        {
            StringBuilder fen = new StringBuilder(88);
            
            fen.Append(ToSmallFen());
            fen.Append(' ');
            AddCurrentPlayerToFen(fen, currentPlayer);
            fen.Append(' ');
            AddCastleToFen(fen);
            fen.Append(' ');
            AddEnPassentToFen(fen, currentPlayer);

            return fen.ToString();
        }

        private void AddCurrentPlayerToFen(StringBuilder fen, PlayerColor currentPlayer)
        {
            if (currentPlayer == PlayerColor.White)
            {
                fen.Append('w');
            }
            else
            {
                fen.Append('b');
            }
        }

        private void AddEnPassentToFen(StringBuilder fen, PlayerColor currentPlayer)
        {
            if (!CanCaptureEnPassant(currentPlayer))
            {
                fen.Append('-');
                return;
            }

            Position pos = GetPawnSkipPosition(currentPlayer.GetOpponent())!;
            fen.Append(pos.ToString());
        }

        private void AddCastleToFen(StringBuilder fen)
        {
            bool castleKSW = CastleRightKS(PlayerColor.White);
            bool castleKSB = CastleRightKS(PlayerColor.Black);
            bool castleQSW = CastleRightQS(PlayerColor.White);
            bool castleQSB = CastleRightQS(PlayerColor.Black);

            if (!(castleKSW || castleKSB || castleQSW || castleQSB))
            {
                fen.Append('-');
                return;
            }

            if (castleKSW)
            {
                fen.Append('K');
            }
            if (castleQSW)
            {
                fen.Append('Q');
            }
            if (castleKSB)
            {
                fen.Append('k');
            }
            if (castleQSB)
            {
                fen.Append('q');
            }
        }

        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            if (IsEmpty(kingPos) || IsEmpty(rookPos))
            {
                return false;
            }

            Piece king = this[kingPos]!;
            Piece rook = this[rookPos]!;

            return king.Type == PieceType.King &&
                !king.HasMoved &&
                rook.Type == PieceType.Rook &&
                !rook.HasMoved;
        }

        public bool CastleRightKS(PlayerColor color)
        {
            return color switch
            {
                PlayerColor.White => IsUnmovedKingAndRook(new Position(4, 7), new Position(7, 7)),
                PlayerColor.Black => IsUnmovedKingAndRook(new Position(4, 0), new Position(7, 0)),
                _ => false
            };
        }

        public bool CastleRightQS(PlayerColor color)
        {
            return color switch{
                PlayerColor.White => IsUnmovedKingAndRook(new Position(4, 7), new Position(0, 7)),
                PlayerColor.Black => IsUnmovedKingAndRook(new Position(4, 0), new Position(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInPosition(PlayerColor color, Position[] pawnPositions, Position skipPos)
        {
            foreach (Position pos in pawnPositions.Where(Contains))
            {
                Piece piece = this[pos]!;
                if (piece == null || piece.Color != color || piece.Type != PieceType.Pawn)
                {
                    continue;
                }
                
                EnPassant move = new EnPassant(pos, skipPos);
                if (move.IsLegal(this))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanCaptureEnPassant(PlayerColor color)
        {
            Position? skipPos = GetPawnSkipPosition(color.GetOpponent());

            if (skipPos == null)
            {
                return false;
            }

            Position[] pawnPosition = color switch
            {
                PlayerColor.White => new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast },
                PlayerColor.Black => new Position[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast },
                _ => Array.Empty<Position>()
            };

            return HasPawnInPosition(color, pawnPosition, skipPos);
        }
    }
}
