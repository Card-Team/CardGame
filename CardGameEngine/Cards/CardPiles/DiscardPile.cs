using System.Collections.Generic;
using CardGameEngine.EventSystem;
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

        // TODO 
        internal DiscardPile(EventManager eventManager) : base(eventManager)
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

        public bool IsMarkedForUpgrade(Card card)
        {
            return MarkedForUpgrade.Contains(card);
        }
    }
}