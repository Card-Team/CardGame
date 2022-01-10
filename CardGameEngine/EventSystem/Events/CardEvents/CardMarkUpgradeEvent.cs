using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'indication de la future amélioration d'une carte
    /// </summary>
    public class CardMarkUpgradeEvent : TransferrableCardEvent
    {
        internal CardMarkUpgradeEvent(Card card) : base(card)
        {
        }
    }
}