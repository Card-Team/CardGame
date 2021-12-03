using CardGameEngine.EventSystem.Events;

// ReSharper disable InconsistentNaming

namespace CardGameEngine.EventSystem
{
    public class EventProperty<S, T, ET> where ET : IPropertyChangeEvent<S, T>
    {
        private S _sender;

        public T Value { get; private set; }


        public EventProperty(S sender)
        {
            throw new System.NotImplementedException();
        }

        public void TryChangeValue(T newVal)
        {
            throw new System.NotImplementedException();
        }
    }
}