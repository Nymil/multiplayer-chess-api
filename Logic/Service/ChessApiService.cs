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
        private static readonly Lazy<ChessApiService> _instance = new Lazy<ChessApiService>(() => new ChessApiService());
        private IEnumerable<ChessGame> _games = new List<ChessGame>();
        public static ChessApiService Instance = _instance.Value;
        private ChessApiService() {}

        public IEnumerable<ChessGame> GetGames()
        {
            return _games;
        }
    }
}
