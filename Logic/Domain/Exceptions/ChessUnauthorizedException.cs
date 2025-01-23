using System;

namespace Logic.Domain.Exceptions;

public class ChessUnauthorizedException : ChessGameException
{
    private static readonly int _CODE = 401;
    public ChessUnauthorizedException(string message) : base(message) {
        Code = _CODE;
    }
}
