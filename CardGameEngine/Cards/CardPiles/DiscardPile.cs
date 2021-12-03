using System.Collections.Generic;

namespace CardGameEngine.Cards.CardPiles
{
    public class DiscardPile : CardPile
    {
        public List<Card> MarkedForUpgrade { get; set; }
    }
}