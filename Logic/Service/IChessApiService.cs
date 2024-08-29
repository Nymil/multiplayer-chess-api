using Logic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Service
{
    public interface IChessApiService
    {
        IEnumerable<ChessGame> GetGames();
    }
}
