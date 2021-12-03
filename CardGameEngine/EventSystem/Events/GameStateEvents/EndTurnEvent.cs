using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    public class EndTurnEvent : Event
    {
        public Player Player { get; set; }
    }
}