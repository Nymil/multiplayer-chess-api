using System;

namespace MultiplayerChessApi.Response;

public class GameByIdResponse
{
    public required string GameId { get; init; }
    public required string Board { get; init; }
    public required string State { get; init; }
    public required string CurrentPlayer { get; init; }
    public required List<string> Players { get; init; }
}
