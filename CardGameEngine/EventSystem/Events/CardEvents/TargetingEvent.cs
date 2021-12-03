using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    public class TargetingEvent : Event
    {
        public Target TargetData { get; set; }
    }
}