using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    public abstract class CardEvent : CancellableEvent
    {
        public Card Card { get; set; }
    }
}