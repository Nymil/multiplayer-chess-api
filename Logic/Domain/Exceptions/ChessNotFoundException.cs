using System;

namespace Logic.Domain.Exceptions;

public class ChessNotFoundException : ChessGameException
{
    private static readonly int _CODE = 404;
    public ChessNotFoundException(string message) : base(message) {
        Code = _CODE;
    }
}
