using System;
using System.Xml;

namespace MultiplayerChessApi.Response;

public class GameByIdResponse
{
    public required string GameId { get; init; }
    public required string Board { get; init; }
    public required string State { get; init; }
    public required string CurrentPlayer { get; init; }
    public required List<string> Players { get; init; }
    public required string? LastMove { get; init; }
    public required GameResultResponse? Result { get; init; }
    public required CapturesResponse CapturedPieces { get; init; }
}

public class CapturesResponse
{
    public required List<string> White { get; init; } 
    public required List<string> Black { get; init; }
}


public class GameResultResponse
{
    public required string? Winner { get; init; }
    public required string Reason { get; init; }
}
