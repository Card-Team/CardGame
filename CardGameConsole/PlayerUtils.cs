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
                var defausse = group.Key == PileIdentifier.CurrentPlayerDiscard ||
                               group.Key == PileIdentifier.OtherPlayerDiscard;
                Table table = GetDefaultTable(group.Key.Display(), defausse);

                foreach (var (card, visible) in group.WithVisionInfo())
                {
                    List<Markup> cols = new List<Markup>();
                    if (visible)
                    {
                        if (defausse)
                        {
                            cols.Add(new Markup($"[grey]{card.Name}[/]"));
                            cols.Add(new Markup($"[grey]{card.Cost}[/]"));
                            cols.Add(new Markup($"[grey]{card.CurrentLevel}/{card.MaxLevel}[/]"));
                            cols.Add(new Markup($"[grey]{card.Description}[/]"));
                        }
                        else
                        {
                            var precondition = card.CanBePlayed(ConsoleGame.Game.CurrentPlayer);
                            var cost2Much = card.Cost.Value > ConsoleGame.Game.CurrentPlayer.ActionPoints.Value;
                            var maxLevel = card.CurrentLevel.Value == card.MaxLevel;

                            cols.Add(precondition && !cost2Much
                                ? new Markup($"{card.Name}")
                                : new Markup($"[red]{card.Name}[/]"));

                            cols.Add(!cost2Much
                                ? new Markup($"{card.Cost}")
                                : new Markup($"[red]{card.Cost}[/]"));

                            cols.Add(!maxLevel
                                ? new Markup($"{card.CurrentLevel}/{card.MaxLevel}")
                                : new Markup($"[yellow]{card.CurrentLevel}/{card.MaxLevel}[/]"));

                            cols.Add(precondition
                                ? new Markup($"{card.Description}")
                                : new Markup($"[red]{card.Description}[/]"));
                        }
                    }
                    else
                    {
                        cols.Add(new Markup("[silver]Caché[/]"));
                        cols.Add(new Markup("[silver]?[/]"));
                        cols.Add(new Markup("[silver]?/?[/]"));
                        cols.Add(new Markup("[silver]Inconnue[/]"));
                    }

                    if (group.Key == PileIdentifier.CurrentPlayerDiscard)
                    {
                        cols.Add(new Markup(ConsoleGame.Game.CurrentPlayer.Discard.IsMarkedForUpgrade(card)
                            ? "[grey]×[/]"
                            : " "));
                    }
                    else if (group.Key == PileIdentifier.OtherPlayerDiscard)
                    {
                        cols.Add(new Markup(
                            ConsoleGame.Game.CurrentPlayer.OtherPlayer.Discard.IsMarkedForUpgrade(card)
                                ? "[grey]×[/]"
                                : " "));
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

        public static IEnumerable<Card> Playable(this IEnumerable<Card> cards)
        {
            return cards.Where(c => c.CanBePlayed(ConsoleGame.Game.CurrentPlayer));
        }

        public static IEnumerable<Card> Upgradable(this IEnumerable<Card> cards)
        {
            return cards.Where(c => c.CurrentLevel.Value < c.MaxLevel);
        }

        public static IEnumerable<Card> CostPlayable(this IEnumerable<Card> cards)
        {
            return cards.Where(c => c.Cost.Value <= ConsoleGame.Game.CurrentPlayer.ActionPoints.Value);
        }
    }
}