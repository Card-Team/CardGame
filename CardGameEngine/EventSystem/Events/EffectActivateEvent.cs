using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events
{
    public class EffectActivateEvent : CancellableEvent
    {
        public Effect Effect { get; set; }
    }
}