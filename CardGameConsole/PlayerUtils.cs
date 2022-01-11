using System;
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
                    Markup[] cols;
                    if (visible)
                    {
                        var precondition = card.CanBePlayed(ConsoleGame.Game, ConsoleGame.Game.CurrentPlayer);
                        var cost2Much = card.Cost.Value > ConsoleGame.Game.CurrentPlayer.ActionPoints.Value;
                        var level = card.CurrentLevel.Value == card.MaxLevel;

                        cols = new[]
                        {
                            new Markup((!precondition || cost2Much ? "[red]" : "")
                                       + card.Name.Value +
                                       (!precondition || cost2Much ? "[/]" : "")),

                            new Markup((cost2Much ? "[red]" : "")
                                       + card.Cost +
                                       (cost2Much ? "[/]" : "")),

                            new Markup((level ? "[yellow]" : "")
                                       + $"{card.CurrentLevel}/{card.MaxLevel}" +
                                       (level ? "[/]" : "")),

                            new Markup((!precondition ? "[red]" : "")
                                       + card.Description.Value +
                                       (!precondition ? "[/]" : ""))
                        };
                    }
                    else
                        cols = new[]
                        {
                            new Markup("[silver]Caché[/]"),
                            new Markup("[silver]?[/]"),
                            new Markup("[silver]?/?[/]"),
                            new Markup("[silver]Inconnue[/]")
                        };

                    if (group.Key == PileIdentifier.CurrentPlayerDiscard)
                    {
                        cols = cols.Append(
                                new Markup(ConsoleGame.Game.CurrentPlayer.Discard.IsMarkedForUpgrade(card) ? "×" : " "))
                            .ToArray();
                    }
                    else if (group.Key == PileIdentifier.OtherPlayerDiscard)
                    {
                        cols = cols.Append(new Markup(
                            ConsoleGame.Game.CurrentPlayer.OtherPlayer.Discard.IsMarkedForUpgrade(card)
                                ? "×"
                                : " ")).ToArray();
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
            return cards.Where(c => c.CanBePlayed(ConsoleGame.Game, ConsoleGame.Game.CurrentPlayer));
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