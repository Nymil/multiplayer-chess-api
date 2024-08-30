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

        public Piece this[Position position]
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
            AddNonPawnPiecesForColor(PlayerColor.White);
            AddNonPawnPiecesForColor(PlayerColor.Black);
            AddNonPawnPieces();
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
    }
}
