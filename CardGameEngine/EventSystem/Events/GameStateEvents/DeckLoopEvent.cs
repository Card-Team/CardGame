using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    public class DeckLoopEvent : Event
    {
        public Player Player { get; set; }
    }
}