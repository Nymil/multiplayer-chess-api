using Logic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Service
{
    public class ChessApiService : IChessApiService
    {
        private IEnumerable<ChessGame> _games = new List<ChessGame>();

        public IEnumerable<ChessGame> GetGames()
        {
            return _games;
        }
    }
}
