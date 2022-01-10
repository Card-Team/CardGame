using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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