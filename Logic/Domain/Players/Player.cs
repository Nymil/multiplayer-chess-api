using System;

namespace Logic.Domain.Players;

public class Player(string username)
{
    public string Uuid { get; } = Guid.NewGuid().ToString();
    public string Username { get; init; } = username;
}
