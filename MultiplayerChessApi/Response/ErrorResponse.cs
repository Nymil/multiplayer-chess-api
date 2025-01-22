using System;

namespace MultiplayerChessApi.Response;

public class ErrorResponse
{
    public required string Error { get; init; }
    public required int Code { get; init; }
}
