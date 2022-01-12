using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'enlevage de l'amélioration future
    /// </summary>
    public class CardUnMarkUpgradeEvent : CardEvent
    {
        internal CardUnMarkUpgradeEvent(Card card) : base(card)
        {
        }
    }
}