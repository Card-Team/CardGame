using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'amélioration d'une carte
    /// </summary>
    public class CardUpgradeEvent : CardEvent
    {
        internal CardUpgradeEvent(Card card) : base(card)
        {
        }
    }
}