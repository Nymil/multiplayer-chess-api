using System;
using Logic.Domain.Pieces;
using Logic.Domain.Players;

namespace Logic.Domain.BoardUtil;

public class Counting
{
    private readonly Dictionary<PieceType, int> _whiteCount = new();
    private readonly Dictionary<PieceType, int> _blackCount = new();

    public int TotalCount { get; private set; }

    public Counting()
    {
        foreach (PieceType type in Enum.GetValues<PieceType>())
        {
            _whiteCount[type] = 0;
            _blackCount[type] = 0;
        }
    }

    public void Increment(PlayerColor color, PieceType type)
    {
        if (color == PlayerColor.White)
        {
            _whiteCount[type]++;
        }
        else
        {
            _blackCount[type]++;
        }

        TotalCount++;
    }

    public int White(PieceType type)
    {
        return _whiteCount[type];
    }

    public int Black(PieceType type)
    {
        return _blackCount[type];
    }
}
