using System.Collections.Generic;

public static class GameState
{
    public static ISet<int> ActivePlayers { get; } = new HashSet<int>();
    public static int WinningPlayer { get; set; } = -1;
}