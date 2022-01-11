using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;

namespace CardGameConsole
{
    public enum PileIdentifier
    {
        Player1Deck,
        Player1Hand,
        Player1Discard,
        Player2Deck,
        Player2Hand,
        Player2Discard,
        Unknown
    }
    public static class PileUtils
    {

        public static string Display(this PileIdentifier pileIdentifier)
        {
            return pileIdentifier switch
            {
                PileIdentifier.Player1Deck => IsCurrent("Votre deck","Le deck adverse"),
                PileIdentifier.Player1Hand => IsCurrent("Votre main","La main adverse"),
                PileIdentifier.Player1Discard => IsCurrent("Votre défausse","La défausse adverse"),
                PileIdentifier.Player2Deck => IsCurrent("Le deck adverse","Votre deck"),
                PileIdentifier.Player2Hand => IsCurrent("La main adverse","Votre main"),
                PileIdentifier.Player2Discard => IsCurrent("La défausse adverse","Votre défausse"),
                PileIdentifier.Unknown => "Inconnu",
                _ => throw new ArgumentOutOfRangeException(nameof(pileIdentifier), pileIdentifier, null)
            };
        }

        private static string IsCurrent(string votreDeck, string leDeckAdverse)
        {
            return ConsoleGame.Game.CurrentPlayer == ConsoleGame.Game.Player1 ? votreDeck : leDeckAdverse;
        }

        public static IEnumerable<IGrouping<PileIdentifier, Card>> SplitIntoPiles(this IEnumerable<Card> cardList)
        {
            return cardList.GroupBy(c =>
            {
                if (ConsoleGame.Game.Player1.Hand.Contains(c))
                {
                    return PileIdentifier.Player1Hand;
                }

                if (ConsoleGame.Game.Player1.Deck.Contains(c))
                {
                    return PileIdentifier.Player1Deck;
                }

                if (ConsoleGame.Game.Player1.Discard.Contains(c))
                {
                    return PileIdentifier.Player1Discard;
                }

                if (ConsoleGame.Game.Player2.Hand.Contains(c))
                {
                    return PileIdentifier.Player2Hand;
                }

                if (ConsoleGame.Game.Player2.OtherPlayer.Deck.Contains(c))
                {
                    return PileIdentifier.Player2Deck;
                }

                if (ConsoleGame.Game.Player2.OtherPlayer.Discard.Contains(c))
                {
                    return PileIdentifier.Player2Discard;
                }

                return PileIdentifier.Unknown;
            });
        }

        public static IEnumerable<(Card,bool)> WithVisionInfo(this IEnumerable<Card> cardList ,Player player)
        {
            var seeable = player.Cards.Concat(player.OtherPlayer.Discard).ToList();
            return cardList.Select(c => (c, seeable.Contains(c)));
        }
    }
}