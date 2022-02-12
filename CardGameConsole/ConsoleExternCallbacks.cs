using System;
using System.Collections.Generic;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;
using Spectre.Console;

namespace CardGameConsole
{
    public class ConsoleExternCallbacks : IExternCallbacks
    {
        private readonly Random _random = new Random();

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
                { ConsoleGame.Player1Name, ConsoleGame.Game.Player1 },
                { ConsoleGame.Player2Name, ConsoleGame.Game.Player2 }
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
            return InputUtils.ChooseFrom(player, card, false, false)!;
        }

        public bool ExternChainOpportunity(Player player)
        {
            var res = InputUtils.ChooseList($"{player.GetName()}, voulez vous chainer ?",
                new Dictionary<string, int> { { "oui", 1 }, { "non", 2 } });
            if (res == 1) return ConsoleGame.DoTurnPrompt(player, true);

            return false;
        }

        public void DebugPrint(string from, string source, string debugPrint)
        {
            EventDisplayer.Events.Add(
                $"[grey][underline][[{Markup.Escape(from)}:{Markup.Escape(source)}]][/] {Markup.Escape(debugPrint)}[/]");
        }

        public int GetExternalRandomNumber(int a, int b)
        {
            return _random.Next(a, b);
        }

        public void ExternGameEnded(Player winner)
        {
            ConsoleGame.Winner = winner;
        }
    }
}