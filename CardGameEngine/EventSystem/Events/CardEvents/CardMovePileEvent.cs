using CardGameEngine.Cards.CardPiles;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    public class CardMovePileEvent : CardEvent
    {
        public CardPile SourcePile { get; set; }

        public CardPile DestPile { get; set; }
    }
}