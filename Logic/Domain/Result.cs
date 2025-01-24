using System;
using Logic.Domain.Players;

namespace Logic.Domain;

public class Result
{
    public Player? Winner { get; init; }
    public EndReason Reason { get; init; }

    private Result(Player? winner, EndReason reason)
    {
        Winner = winner;
        Reason = reason;
    }

    public static Result Win(Player winner)
    {
        return new Result(winner, EndReason.Checkmate);
    }

    public static Result Draw(EndReason reason)
    {
        return new Result(null, reason);
    }
}
