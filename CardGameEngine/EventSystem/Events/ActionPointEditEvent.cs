using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events
{
    public class ActionPointEditEvent : CancellableEvent, IPropertyChangeEvent<Player, int>
    {
        public Player Player { get; set; }

        public int OldPointCount { get; private set; }
        public int NewPointCount { get; set; }

        Player IPropertyChangeEvent<Player, int>.Sender
        {
            get => Player;
            set => Player = value;
        }

        int IPropertyChangeEvent<Player, int>.NewValue
        {
            get => NewPointCount;
            set => NewPointCount = value;
        }

        int IPropertyChangeEvent<Player, int>.OldValue
        {
            get => OldPointCount;
            set => OldPointCount = value;
        }
    }
}