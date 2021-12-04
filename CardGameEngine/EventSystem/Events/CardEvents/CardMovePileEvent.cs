using CardGameEngine.Cards.CardPiles;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    public class CardMovePileEvent : CardEvent
    {
        public CardPile SourcePile { get; set; }
        public int SourceIndex { get; set; }

        public CardPile DestPile { get; set; }
        public int DestIndex { get; set; }
    }
}