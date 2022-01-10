using System.Collections.Generic;
using CardGameEngine.EventSystem;

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
        public HashSet<Card> MarkedForUpgrade { get; }

        // TODO 
        internal DiscardPile(EventManager eventManager) : base(eventManager)
        {
            MarkedForUpgrade = new HashSet<Card>();
        }
    }
}