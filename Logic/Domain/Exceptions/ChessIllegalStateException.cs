using System;

namespace Logic.Domain.Exceptions;

public class ChessIllegalStateException : ChessGameException
{
    private static readonly int _CODE = 405;
    public ChessIllegalStateException(string message) : base(message) {
        Code = _CODE;
    }
}
