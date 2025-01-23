using System;

namespace Logic.Domain.Players;

public class Player(string username, PlayerColor color)
{
    public string Uuid { get; } = Guid.NewGuid().ToString();
    public string Username { get; init; } = username;

    public PlayerColor Color { get; init; } = color;
}
