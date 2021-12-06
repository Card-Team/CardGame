using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.KeywordEvents
{
    public class CardKeywordEvent : CardEvent
    {
        public Keyword Keyword { get; set; }
    }
}