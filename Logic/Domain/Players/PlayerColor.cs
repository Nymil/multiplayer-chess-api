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
        public static PlayerColor GetOpponent(this PlayerColor playerColor)
        {
            return playerColor switch
            {
                PlayerColor.White => PlayerColor.Black,
                PlayerColor.Black => PlayerColor.White,
                _ => throw new ArgumentOutOfRangeException(nameof(playerColor), playerColor, "Invalid player color.")
            };
        }
    }
        
}
