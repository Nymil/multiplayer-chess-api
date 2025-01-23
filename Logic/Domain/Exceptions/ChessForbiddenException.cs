using System;

namespace Logic.Domain.Exceptions;

public class ChessForbidenException : ChessGameException
{
    private static readonly int _CODE = 403;
    public ChessForbidenException(string message) : base(message) {
        Code = _CODE;
    }
}
