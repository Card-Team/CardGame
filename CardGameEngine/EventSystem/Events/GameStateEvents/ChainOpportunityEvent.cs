using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    public class ChainOpportunityEvent : CancellableEvent
    {
        public Player Chainer { get; internal set; }

        public ChainOpportunityEvent(Player chainer)
        {
            Chainer = chainer;
        }
    }
}