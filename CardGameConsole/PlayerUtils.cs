using System;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;

namespace CardGameConsole
{
    public static class PlayerUtils
    {
        public static string GetName(this Player player)
        {
            return player == ConsoleGame.Game.Player1 ? ConsoleGame.Player1Name : ConsoleGame.Player2Name;
        }

        public static void PrintHand(this Player player)
        {
            for (var i = 0; i < player.Hand.Count; i++)
            {
                Console.WriteLine($"{i} - {player.Hand[i]}");
            }
        }

        public static void PrintDiscard(this Player player)
        {
            for (var i = 0; i < player.Discard.Count; i++)
            {
                Console.Write($"{i} - {player.Discard[i]}");
                if (player.Discard.IsMarkedForUpgrade(player.Discard[i]))
                {
                    Console.Write(" [Marquée]\n");
                }
            }
        }
    }
}