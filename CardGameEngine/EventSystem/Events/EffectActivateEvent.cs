using CardGameEngine.GameSystems;
using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine.EventSystem.Events
{
    public class EffectActivateEvent : CancellableEvent
    {
        public Effect Effect { get; set; }
    }
}