using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    public class StartTurnEvent : Event
    {
        public Player Player { get; set; }
    }
}