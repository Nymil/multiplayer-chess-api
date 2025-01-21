using System;

namespace Logic.Domain.Exceptions;

public class ChessGameException : Exception
{
    public int Code { get; init; }
    public ChessGameException(string message) : base(message) { }

}
