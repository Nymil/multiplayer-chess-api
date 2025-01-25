using System;

namespace MultiplayerChessApi.Response;

public class ValidMovesResponse
{
    // hashset for only unique moves to be shown
    public required HashSet<string> ValidMoves { get; init; }
}
