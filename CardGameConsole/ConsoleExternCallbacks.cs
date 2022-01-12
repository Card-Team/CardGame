using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardGameEngine;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;
using MoonSharp.Interpreter;
using Spectre.Console;

namespace CardGameConsole
{
    public class ConsoleExternCallbacks : IExternCallbacks
    {
        public Card ExternCardAskForTarget(Player effectOwner, string targetName, List<Card> cardList)
        {
            Console.WriteLine($"Choisissez une carte pour {targetName}");

            var result = InputUtils.ChooseFrom(effectOwner, cardList);

            return result;
        }

        public Player ExternPlayerAskForTarget(Player effectOwner, string targetName)
        {
            return InputUtils.ChooseList($"Veuillez choisir un joueur pour {targetName}", new Dictionary<string, Player>
            {
                {ConsoleGame.Player1Name, ConsoleGame.Game.Player1},
                {ConsoleGame.Player2Name, ConsoleGame.Game.Player2}
            });
        }

        public void ExternShowCard(Player player, Card card)
        {
            if (player != ConsoleGame.Game.CurrentPlayer)
            {
                AnsiConsole.Clear();
                AnsiConsole.Ask($"Veuillez passer la main a [bold]{player.GetName()}[/] temporairement", "");
            }

            AnsiConsole.Ask($"La carte suivante vous est montrée : {card}", "");

            if (player != ConsoleGame.Game.CurrentPlayer)
                AnsiConsole.Ask($"Veuillez rendre la main a [bold]{ConsoleGame.Game.CurrentPlayer.GetName()}[/]", "");

            AnsiConsole.Clear();
        }

        public Card ExternChooseBetween(Player player, List<Card> card)
        {
            return InputUtils.ChooseList("Veuillez faire un choix", card);
        }

        public void DebugPrint(string from,string source, string debugPrint)
        {
            EventDisplayer.Events.Add($"[grey][underline][[{Markup.Escape(from)}:{Markup.Escape(source)}]][/] {Markup.Escape(debugPrint)}[/]");
        }

        public void ExternGameEnded(Player winner)
        {
            ConsoleGame.Winner = winner;
        }
    }
}