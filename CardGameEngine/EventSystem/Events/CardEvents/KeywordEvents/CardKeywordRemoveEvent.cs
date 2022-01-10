using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.KeywordEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'enlèvement d'un mot-clé sur une carte
    /// </summary>
    public class CardKeywordRemoveEvent : CardKeywordEvent
    {
        internal CardKeywordRemoveEvent(Card card, Keyword keyword) : base(card, keyword)
        {
        }
    }
}