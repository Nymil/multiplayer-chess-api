using Logic.Domain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain
{
    public class Board
    {
        private readonly Piece[,] _pieces = new Piece[8, 8];

        public Piece this[int col, int row]
        {
            get { return _pieces[col, row]; }
            set { _pieces[col, row] = value; }
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

        private void AddNonPawnPiecesForColor(PlayerColor color)
        {
            int row = color == PlayerColor.White ? 0 : 7;
            this[row, 0] = new Rook(color);
            this[row, 1] = new Knight(color);
            this[row, 2] = new Bishop(color);
            this[row, 3] = new Queen(color);
            this[row, 4] = new King(color);
            this[row, 5] = new Bishop(color);
            this[row, 6] = new Knight(color);
            this[row, 7] = new Rook(color);
        }

        private void AddNonPawnPieces()
        {
            for (int col = 0; col < 8; col++)
            {
                this[1, col] = new Pawn(PlayerColor.White);
                this[6, col] = new Pawn(PlayerColor.Black);
            }
        }
    }
}
