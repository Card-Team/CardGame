using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.KeywordEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'activation d'un mot-clé sur une carte
    /// </summary>
    public class CardKeywordTriggerEvent : CardKeywordEvent
    {
        internal CardKeywordTriggerEvent(Card card, Keyword keyword) : base(card, keyword)
        {
        }
    }
}