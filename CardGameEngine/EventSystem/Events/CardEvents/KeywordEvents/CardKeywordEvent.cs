using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.KeywordEvents
{
    public class CardKeywordEvent : CardEvent
    {
        public Card Card { get; set; }

        public Keyword Keyword { get; set; }
    }
}