using System;

namespace MultiplayerChessApi.Response;

public class GameCreatedResponse
{
    public required string GameId { get; init; }
}
