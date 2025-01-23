using System;

namespace Logic.Domain.Exceptions;

public class ChessBadRequestException : ChessGameException
{
    private static readonly int _CODE = 400;
    public ChessBadRequestException(string message) : base(message) {
        Code = _CODE;
    }
}
