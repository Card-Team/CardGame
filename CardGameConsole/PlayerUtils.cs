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
            Console.WriteLine("Cartes dans votre main : ");
            foreach (Card card in player.Hand)
            {
                Console.WriteLine(card);
            }
        } 
    }
}