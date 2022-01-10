using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.PropertyChange
{
    /// <summary>
    /// Évènement annulable correspondant au changement de niveau d'une carte
    /// </summary>
    public class CardLevelChangeEvent : CardPropertyChangeEvent<int>
    {
    }
}