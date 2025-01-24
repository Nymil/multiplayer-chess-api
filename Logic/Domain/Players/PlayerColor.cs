using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain.Players
{
    public enum PlayerColor
    {
        White,
        Black
    }

    public static class PlayerColorExtensions
    {
        public static PlayerColor GetOpponent(this PlayerColor color)
        {
            return color == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
        }
    }
}
