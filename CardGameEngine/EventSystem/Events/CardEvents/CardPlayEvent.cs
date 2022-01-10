using CardGameEngine.Cards;
using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'utilisation d'une carte
    /// </summary>
    public class CardPlayEvent : CardEvent
    {
        public Player WhoPlayed { get; internal set; }

        internal CardPlayEvent(Player whoPlayed,Card card) : base(card)
        {
            WhoPlayed = whoPlayed;
        }
    }
}