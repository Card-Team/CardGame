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
            return pileIdentifier switch
            {
                PileIdentifier.CurrentPlayerDeck => "Votre deck",
                PileIdentifier.CurrentPlayerHand => "Votre main",
                PileIdentifier.CurrentPlayerDiscard => "Votre défausse",
                PileIdentifier.OtherPlayerDeck => "Le deck adverse",
                PileIdentifier.OtherPlayerHand => "La main adverse",
                PileIdentifier.OtherPlayerDiscard => "La défausse adverse",
                PileIdentifier.Unknown => "Inconnu",
                _ => throw new ArgumentOutOfRangeException(nameof(pileIdentifier), pileIdentifier, null)
            };
        }

        public static PileIdentifier Identifier(this CardPile pile, Player? pov = null)
        {
            pov ??= ConsoleGame.Game.CurrentPlayer;
            if (pile == pov.Hand)
            {
                return PileIdentifier.CurrentPlayerHand;
            }

            if (pile == pov.Deck)
            {
                return PileIdentifier.CurrentPlayerDeck;
            }

            if (pile == pov.Discard)
            {
                return PileIdentifier.CurrentPlayerDiscard;
            }

            if (pile == pov.OtherPlayer.Hand)
            {
                return PileIdentifier.OtherPlayerHand;
            }

            if (pile == pov.OtherPlayer.Deck)
            {
                return PileIdentifier.OtherPlayerDeck;
            }

            if (pile == pov.OtherPlayer.Discard)
            {
                return PileIdentifier.OtherPlayerDiscard;
            }

            return PileIdentifier.Unknown;
        }

        public static IEnumerable<IGrouping<PileIdentifier, Card>> SplitIntoPiles(this IEnumerable<Card> cardList)
        {
            return cardList.GroupBy(c =>
            {
                if (ConsoleGame.Game.CurrentPlayer.Hand.Contains(c))
                {
                    return PileIdentifier.CurrentPlayerHand;
                }

                if (ConsoleGame.Game.CurrentPlayer.Deck.Contains(c))
                {
                    return PileIdentifier.CurrentPlayerDeck;
                }

                if (ConsoleGame.Game.CurrentPlayer.Discard.Contains(c))
                {
                    return PileIdentifier.CurrentPlayerDiscard;
                }

                if (ConsoleGame.Game.CurrentPlayer.OtherPlayer.Hand.Contains(c))
                {
                    return PileIdentifier.OtherPlayerHand;
                }

                if (ConsoleGame.Game.CurrentPlayer.OtherPlayer.Deck.Contains(c))
                {
                    return PileIdentifier.OtherPlayerDeck;
                }

                if (ConsoleGame.Game.CurrentPlayer.OtherPlayer.Discard.Contains(c))
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