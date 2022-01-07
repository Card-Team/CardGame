using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents.PropertyChange
{
    /// <summary>
    /// Classe représentant les évènements liés à la modification d'une propriété d'une carte
    /// </summary>
    /// <typeparam name="T">Le type de la valeur changée</typeparam>
    public class CardPropertyChangeEvent<T> : CardEvent, IPropertyChangeEvent<Card, T>
    {
        /// <summary>
        /// Ancienne valeur de la propriété
        /// </summary>
        public T OldValue { get; private set; }

        /// <summary>
        /// Nouvelle valeur de la propriété
        /// </summary>
        public T NewValue { get; private set; }

        T IPropertyChangeEvent<Card, T>.OldValue
        {
            get => OldValue;
            set => OldValue = value;
        }

        T IPropertyChangeEvent<Card, T>.NewValue
        {
            get => NewValue;
            set => NewValue = value;
        }

        Card IPropertyChangeEvent<Card, T>.Sender
        {
            get => Card;
            set => Card = value;
        }
    }
}