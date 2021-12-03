namespace CardGameEngine.EventSystem.Events
{
    public interface IPropertyChangeEvent<S, T>
    {
        public S Sender { get; set; }

        public T NewValue { get; set; }

        public T OldValue { get; set; }
    }
}