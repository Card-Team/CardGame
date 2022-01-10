using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.KeywordEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'ajout d'un mot-clé sur une carte
    /// </summary>
    public class CardKeywordAddEvent : CardKeywordEvent
    {
        internal CardKeywordAddEvent(Card card, Keyword keyword) : base(card, keyword)
        {
        }
    }
}