using CardGameEngine.Events;

namespace CardGameEngine.EventSystem.Events
{
    public abstract class CancellableEvent : Event
    {
        public bool Cancelled { get; set; }
    }
}