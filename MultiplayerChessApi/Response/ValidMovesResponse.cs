using System;

namespace MultiplayerChessApi.Response;

public class ValidMovesResponse
{
    public required string[] ValidMoves { get; init; }
}
