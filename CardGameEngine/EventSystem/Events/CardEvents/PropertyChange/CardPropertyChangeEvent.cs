using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.PropertyChange
{
    public class CardPropertyChangeEvent<T> : CardEvent, IPropertyChangeEvent<Card, T>
    {
        public T OldValue { get; private set; }

        public T NewValue { get; set; }

        T IPropertyChangeEvent<Card, T>.OldValue
        {
            get => OldValue;
            set => OldValue = value;
        }

        Card IPropertyChangeEvent<Card, T>.Sender
        {
            get => Card;
            set => Card = value;
        }
    }
}