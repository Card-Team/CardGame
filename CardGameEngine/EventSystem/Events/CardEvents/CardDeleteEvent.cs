using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement annulable correspondant à la suppression d'une carte
    /// </summary>
    public class CardDeleteEvent : TransferrableCardEvent
    {
        internal CardDeleteEvent(Card card) : base(card)
        {
        }
    }
}