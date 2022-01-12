using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.Cards.CardPiles;
using CardGameEngine.GameSystems;

namespace CardGameConsole
{
    public enum PileIdentifier
    {
        CurrentPlayerDeck,
        CurrentPlayerHand,
        CurrentPlayerDiscard,
        OtherPlayerDeck,
        OtherPlayerHand,
        OtherPlayerDiscard,
        Unknown
    }

    public static class PileUtils
    {
        public static string Display(this PileIdentifier pileIdentifier, Player? pov = null)
        {
            pov ??= ConsoleGame.Game.CurrentPlayer;
            return pileIdentifier switch
            {
                PileIdentifier.CurrentPlayerDeck => pov.IsCurrent("Votre deck", "Le deck adverse"),
                PileIdentifier.CurrentPlayerHand => pov.IsCurrent("Votre main", "La main adverse"),
                PileIdentifier.CurrentPlayerDiscard => pov.IsCurrent("Votre défausse", "La défausse adverse"),
                PileIdentifier.OtherPlayerDeck => pov.IsCurrent("Le deck adverse", "Votre deck"),
                PileIdentifier.OtherPlayerHand => pov.IsCurrent("La main adverse", "Votre main"),
                PileIdentifier.OtherPlayerDiscard => pov.IsCurrent("La défausse adverse", "Votre défausse"),
                PileIdentifier.Unknown => "Inconnu",
                _ => throw new ArgumentOutOfRangeException(nameof(pileIdentifier), pileIdentifier, null)
            };
        }

        private static string IsCurrent(this Player player, string votreDeck, string leDeckAdverse)
        {
            return ConsoleGame.Game.CurrentPlayer == player ? votreDeck : leDeckAdverse;
        }

        public static PileIdentifier Identifier(this CardPile pile)
        {
            if (pile == ConsoleGame.Game.CurrentPlayer.Hand)
            {
                return PileIdentifier.CurrentPlayerHand;
            }

            if (pile == ConsoleGame.Game.CurrentPlayer.Deck)
            {
                return PileIdentifier.CurrentPlayerDeck;
            }

            if (pile == ConsoleGame.Game.CurrentPlayer.Discard)
            {
                return PileIdentifier.CurrentPlayerDiscard;
            }

            if (pile == ConsoleGame.Game.CurrentPlayer.OtherPlayer.Hand)
            {
                return PileIdentifier.OtherPlayerHand;
            }

            if (pile == ConsoleGame.Game.CurrentPlayer.OtherPlayer.Deck)
            {
                return PileIdentifier.OtherPlayerDeck;
            }

            if (pile == ConsoleGame.Game.CurrentPlayer.OtherPlayer.Discard)
            {
                return PileIdentifier.OtherPlayerDiscard;
            }

            return PileIdentifier.Unknown;
        }

        public static IEnumerable<IGrouping<PileIdentifier, Card>> SplitIntoPiles(this IEnumerable<Card> cardList)
        {
            return cardList.GroupBy(c =>
            {
                if (ConsoleGame.Game.Player1.Hand.Contains(c))
                {
                    return PileIdentifier.CurrentPlayerHand;
                }

                if (ConsoleGame.Game.Player1.Deck.Contains(c))
                {
                    return PileIdentifier.CurrentPlayerDeck;
                }

                if (ConsoleGame.Game.Player1.Discard.Contains(c))
                {
                    return PileIdentifier.CurrentPlayerDiscard;
                }

                if (ConsoleGame.Game.Player2.Hand.Contains(c))
                {
                    return PileIdentifier.OtherPlayerHand;
                }

                if (ConsoleGame.Game.Player2.Deck.Contains(c))
                {
                    return PileIdentifier.OtherPlayerDeck;
                }

                if (ConsoleGame.Game.Player2.Discard.Contains(c))
                {
                    return PileIdentifier.OtherPlayerDiscard;
                }

                return PileIdentifier.Unknown;
            });
        }

        public static IEnumerable<(Card, bool)> WithVisionInfo(this IEnumerable<Card> cardList, Player? pov = null)
        {
            pov ??= ConsoleGame.Game.CurrentPlayer;

            var seeable = pov.Cards.Concat(pov.OtherPlayer.Discard).ToList();
            return cardList.Select(c => (c, seeable.Contains(c)));
        }
    }
}