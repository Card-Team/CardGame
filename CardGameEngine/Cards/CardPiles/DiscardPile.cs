using System.Collections.Generic;
using CardGameEngine.EventSystem.Events.CardEvents;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.Cards.CardPiles
{
    /// <summary>
    /// Pile de cartes représentant une défausse
    /// </summary>
    /// <seealso cref="CardPile"/>
    public class DiscardPile : CardPile
    {
        /// <summary>
        /// Set des cartes à améliorer au prochain roulement
        /// </summary>
        private HashSet<Card> MarkedForUpgrade { get; }

        internal DiscardPile(Game game) : base(game)
        {
            MarkedForUpgrade = new HashSet<Card>();
        }

        [MoonSharpVisible(true)]
        internal bool MoveForUpgrade(CardPile oldLocation, Card toUp)
        {
            var moveResult = oldLocation.MoveTo(this, toUp, 0);
            if (moveResult == false) return false;
            MarkedForUpgrade.Add(toUp);

            return true;
        }

        [MoonSharpVisible(true)]
        internal override bool MoveTo(CardPile newCardPile, Card card, int newPosition)
        {
            var moveResult = base.MoveTo(newCardPile, card, newPosition);
            switch (moveResult)
            {
                case false:
                    return false;
                case true when newCardPile != this:
                    MarkedForUpgrade.Remove(card);
                    break;
            }

            return moveResult;
        }

        [MoonSharpVisible(true)]
        public void UnMarkForUpgrade(Card card)
        {
            var evt = new CardUnMarkUpgradeEvent(card);

            using (var post = EventManager.SendEvent(evt))
            {
                if (post.Event.Cancelled)
                    return;

                MarkedForUpgrade.Remove(post.Event.Card);
            }
        }

        public bool IsMarkedForUpgrade(Card card)
        {
            return MarkedForUpgrade.Contains(card);
        }
    }
}