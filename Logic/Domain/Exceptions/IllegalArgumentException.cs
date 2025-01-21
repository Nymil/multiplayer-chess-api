using System;

namespace Logic.Domain.Exceptions;

public class IllegalArgumentException : ChessGameException
{
    private static readonly int _CODE = 409;
    public IllegalArgumentException(string message) : base(message) {
        Code = _CODE;
    }
}
