using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'utilisation d'une carte
    /// </summary>
    public class CardPlayEvent : CardEvent
    {
        internal CardPlayEvent(Card card) : base(card)
        {
        }
    }
}