using System;

namespace Logic.Domain.Exceptions;

public class IllegalArgumentException : Exception
{
    public IllegalArgumentException(string message) : base(message) { }
}
