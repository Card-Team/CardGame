using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.PropertyChange
{
    /// <summary>
    /// Évènement annulable correspondant au changement de coût d'une carte
    /// </summary>
    public class CardCostChangeEvent : CardPropertyChangeEvent<int>
    {
    }
}