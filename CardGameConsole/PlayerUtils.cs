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
            PrintCardsTable(player.Hand.GroupBy(c => player.Hand.Identifier()));
        }

        public static void PrintDiscard(this Player player)
        {
            PrintCardsTable(player.Discard.GroupBy(c => player.Discard.Identifier()));
        }

        public static void PrintCardsTable(IEnumerable<Card> cards, string title)
        {
            Table table = new Table().Title(title)
                .AddColumn(new TableColumn("Nom").Centered())
                .AddColumn(new TableColumn("Coût").Centered())
                .AddColumn(new TableColumn("Niveau").Centered())
                .AddColumn(new TableColumn("Description").Centered());

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
                Table table = new Table().Title(group.Key.Display())
                    .AddColumn(new TableColumn("Nom").Centered())
                    .AddColumn(new TableColumn("Coût").Centered())
                    .AddColumn(new TableColumn("Niveau").Centered())
                    .AddColumn(new TableColumn("Description").Centered()).Centered();

                foreach (var (card, visible) in group.WithVisionInfo())
                {
                    if (visible)
                        table.AddRow(card.Name.Value, card.Cost.Value.ToString(),
                            $"{card.CurrentLevel}/{card.MaxLevel}",
                            card.Description.Value);
                    else
                        table.AddRow("caché");
                }

                AnsiConsole.Write(table);
            }
        }
    }
}