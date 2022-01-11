using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;
using Spectre.Console;

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
            if (player.Hand.IsEmpty)
                AnsiConsole.WriteLine("La main est vide");
            else
                PrintCardsTable(player.Hand.GroupBy(c => player.Hand.Identifier()));
        }

        public static void PrintDiscard(this Player player)
        {
            if (player.Discard.IsEmpty)
                AnsiConsole.WriteLine("La défausse est vide");
            else
                PrintCardsTable(player.Discard.GroupBy(c => player.Discard.Identifier()));
        }

        public static void PrintCardsTable(IEnumerable<Card> cards, string title)
        {
            Table table = GetDefaultTable(title);

            foreach (var card in cards)
            {
                table.AddRow(card.Name.Value, card.Cost.Value.ToString(), $"{card.CurrentLevel}/{card.MaxLevel}",
                    card.Description.Value);
            }

            AnsiConsole.Write(table);
        }

        private static void PrintCardsTable(IEnumerable<IGrouping<PileIdentifier, Card>> cards)
        {
            foreach (var group in cards)
            {
                Table table = GetDefaultTable(group.Key.Display(),
                    group.Key == PileIdentifier.CurrentPlayerDiscard || group.Key == PileIdentifier.OtherPlayerDiscard);

                foreach (var (card, visible) in group.WithVisionInfo())
                {
                    string[] cols;
                    if (visible)
                        cols = new[]
                        {
                            card.Name.Value, card.Cost.Value.ToString(),
                            $"{card.CurrentLevel}/{card.MaxLevel}",
                            card.Description.Value
                        };
                    else
                        cols = new[]
                        {
                            "Caché",
                            "?",
                            "?/?",
                            "Inconnue"
                        };

                    if (group.Key == PileIdentifier.CurrentPlayerDiscard)
                    {
                        cols = cols.Append(ConsoleGame.Game.CurrentPlayer.Discard.IsMarkedForUpgrade(card) ? "×" : " ")
                            .ToArray();
                    }
                    else if (group.Key == PileIdentifier.OtherPlayerDiscard)
                    {
                        cols = cols.Append(ConsoleGame.Game.CurrentPlayer.OtherPlayer.Discard.IsMarkedForUpgrade(card)
                            ? "×"
                            : " ").ToArray();
                    }

                    table.AddRow(cols);
                }

                AnsiConsole.Write(table);
            }
        }

        private static Table GetDefaultTable(string title, bool upgrade = false)
        {
            Table table = new Table().Title(title)
                .AddColumn(new TableColumn("[bold][underline]Nom[/][/]").Centered())
                .AddColumn(new TableColumn("[bold][underline]Coût[/][/]").Centered())
                .AddColumn(new TableColumn("[bold][underline]Niveau[/][/]").Centered())
                .AddColumn(new TableColumn("[bold][underline]Description[/][/]").Centered())
                .Centered();

            if (upgrade)
                table.AddColumn(new TableColumn("[bold][underline]Marquée[/][/]").Centered());

            return table;
        }
    }
}