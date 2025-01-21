using System;

namespace Logic.Domain.Exceptions;

public class IllegalStateException : ChessGameException
{
    private static readonly int _CODE = 409;
    public IllegalStateException(string message) : base(message) {
        Code = _CODE;
    }
}
