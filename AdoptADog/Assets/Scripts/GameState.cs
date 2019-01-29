using System.Collections.Generic;
using XInputDotNetPure;

public static class GameState
{
    public static ISet<PlayerIndex> ActivePlayers { get; } = new HashSet<PlayerIndex>();
    public static PlayerIndex WinningPlayer { get; set; } = PlayerIndex.One;
}