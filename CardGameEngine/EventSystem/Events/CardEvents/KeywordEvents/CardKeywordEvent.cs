using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.KeywordEvents
{
    /// <summary>
    /// Évènement annulable en lien avec un mot-clé et une carte
    /// </summary>
    public abstract class CardKeywordEvent : CardEvent
    {
        public Keyword Keyword { get; }

        internal CardKeywordEvent(Card card, Keyword keyword) : base(card)
        {
            Keyword = keyword;
        }
    }
}