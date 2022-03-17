using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    public class ChainingEvent : Event
    {
        public Player Chainer { get; internal set; }

        public ChainingEvent(Player chainer)
        {
            Chainer = chainer;
        }
    }
}