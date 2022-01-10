using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Classe abstraite représentant les évènements liés à une carte
    /// </summary>
    public abstract class TransferrableCardEvent : CardEvent
    {
        /// <summary>
        /// La carte concernée
        /// </summary>
        public new Card Card
        {
            get => base.Card;
            internal set => base.Card = value;
        }

        protected TransferrableCardEvent(Card card) : base(card)
        {
        }
    }
}