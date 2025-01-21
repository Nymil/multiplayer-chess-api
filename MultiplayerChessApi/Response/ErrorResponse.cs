using System;

namespace MultiplayerChessApi.Response;

public class ErrorResponse
{
    public string Error { get; set; }
    public int Code { get; set; }

    public ErrorResponse(string error, int code)
    {
        Error = error;
        Code = code;
    }
}
