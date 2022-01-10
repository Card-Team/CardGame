using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Classe abstraite représentant les évènements liés à une carte
    /// </summary>
    public abstract class CardEvent : CancellableEvent
    {
        /// <summary>
        /// La carte concernée
        /// </summary>
        public Card Card { get; internal set; }

        protected CardEvent(Card card)
        {
            Card = card;
        }
    }
}