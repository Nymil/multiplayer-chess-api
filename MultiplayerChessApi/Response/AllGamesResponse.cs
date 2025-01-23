using System;
using Microsoft.OpenApi.Models;

namespace MultiplayerChessApi.Response;

public class AllGamesResponse
{
    public required string Id { get; init; }
    public string? State { get; init; }
}
