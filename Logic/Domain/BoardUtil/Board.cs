using Logic.Domain.Pieces;
using Logic.Domain.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.BoardUtil
{
    public class Board
    {
        private readonly Piece?[,] _pieces = new Piece[8, 8];

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

        public Board()
        {
            AddStartPieces();
        }

        private void AddStartPieces()
        {
            this[4, 7] = new King(PlayerColor.Black);
            this[3, 4] = new Queen(PlayerColor.Black);

            this[1, 5] = new Knight(PlayerColor.White);
            this[3, 0] = new King(PlayerColor.White);
            this[7, 2] = new Rook(PlayerColor.White);

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

        public override string ToString()
        {
            return ToSmallFen();
        }
    }
}
