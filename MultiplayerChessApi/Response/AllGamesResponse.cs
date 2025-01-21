using System;

namespace MultiplayerChessApi.Response;

public class AllGamesResponse
{
    public required string Id { get; init; }
    public required string State { get; init; }
}
