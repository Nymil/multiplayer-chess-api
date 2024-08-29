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
            return;
        }
    }
}
