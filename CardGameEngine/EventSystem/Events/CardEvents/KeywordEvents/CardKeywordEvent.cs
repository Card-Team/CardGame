using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.KeywordEvents
{
    /// <summary>
    /// Évènement annulable en lien avec un mot-clé et une carte
    /// </summary>
    public class CardKeywordEvent : CardEvent
    {
        public Keyword Keyword { get; set; }
    }
}