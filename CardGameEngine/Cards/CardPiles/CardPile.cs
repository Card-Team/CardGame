using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.CardEvents;

namespace CardGameEngine.Cards.CardPiles
{
    /// <summary>
    /// Classe représentant une pile de cartes comme la main, le deck ou la défausse
    /// </summary>
    public class CardPile : IEnumerable<Card>
    {
        /// <summary>
        /// Liste des cartes dans la pile
        /// </summary>
        private List<Card> _cardList;

        /// <summary>
        /// EventManager de la partie
        /// </summary>
        private EventManager EventManager { get; }

        /// <summary>
        /// Nombre de cartes maximal dans la pile
        /// </summary>
        private int? LimitSize { get; }

        /// <summary>
        /// Propriété renvoyant le nombre de cartes
        /// </summary>
        public int Count => _cardList.Count;

        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Accesseur de la liste
        /// </summary>
        /// <param name="i">Index de la carte</param>
        public Card this[int i] => _cardList.ElementAt(i);


        /// <summary>
        /// Constructeur de la pile vide
        /// </summary>
        /// <param name="eventManager">EventManager de la partie</param>
        /// <param name="limitSize">Taille limite de la pile, null pour aucune limite</param>
        internal CardPile(EventManager eventManager, int? limitSize = null)
        {
            EventManager = eventManager;
            _cardList = new List<Card>();
            LimitSize = limitSize;
        }

        /// <summary>
        /// Constructeur de la pile remplie
        /// </summary>
        /// <param name="eventManager">EventManager de la partie</param>
        /// <param name="cards"></param>
        internal CardPile(EventManager eventManager, List<Card> cards)
        {
            EventManager = eventManager;
            _cardList = cards;
            LimitSize = null;
        }


        /// <summary>
        /// Méthode indiquant si la pile contient une certaine carte
        /// </summary>
        /// <param name="card">La carte à chercher</param>
        /// <returns>Un booléen en fonction de la présence de la carte</returns>
        public bool Contains(Card card)
        {
            return _cardList.Contains(card);
        }

        /// <summary>
        /// Méthode renvoyant la position d'une carte dans la pile
        /// </summary>
        /// <param name="card">La carte à chercher</param>
        /// <returns>Un entier selon la position</returns>
        public int IndexOf(Card card)
        {
            return _cardList.IndexOf(card);
        }

        /// <summary>
        /// Insère une carte dans la liste en décalant les autres
        /// </summary>
        /// <param name="newPosition">Nouvelle position</param>
        /// <param name="card">La carte à insérer</param>
        private void Insert(int newPosition, Card card)
        {
            _cardList.Insert(newPosition, card);
        }

        /// <summary>
        /// Renvoie un itérateur pour la liste
        /// </summary>
        /// <returns>L'itérateur créé</returns>
        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return _cardList.GetEnumerator();
        }

        /// <summary>
        /// Renvoie un itérateur pour la liste
        /// </summary>
        /// <returns>L'itérateur créé</returns>
        public IEnumerator GetEnumerator()
        {
            return _cardList.GetEnumerator();
        }

        /// <summary>
        /// Déplace une carte dans une autre pile à une position donnée
        /// </summary>
        /// <param name="newCardPile">La nouvelle ciblée</param>
        /// <param name="card">La carte à bouger</param>
        /// <param name="newPosition">La position à prendre</param>
        /// <exception cref="NotInPileException">Si la carte n'est pas dans la pile</exception>
        internal virtual bool MoveTo(CardPile newCardPile, Card card, int newPosition)
        {
            if (!_cardList.Contains(card)) throw new NotInPileException(card);
            if (Count + 1 > LimitSize) return false;

            CardMovePileEvent moveEvent = new CardMovePileEvent(card, this, IndexOf(card), newCardPile, newPosition);
            using (var postEvent = EventManager.SendEvent(moveEvent))
            {
                if (postEvent.Event.Cancelled)
                {
                    return false;
                }

                if (!_cardList.Remove(postEvent.Event.Card))
                {
                    throw new InvalidOperationException(
                        $"La carte {card} a disparue de la pile {this} pendant l'opération de déplacement vers {postEvent.Event.DestPile}");
                }
                postEvent.Event.DestPile.Insert(postEvent.Event.DestIndex, postEvent.Event.Card);
            }

            return true;
        }

        /// <summary>
        /// Déplace une carte dans sa pile à une position donnée
        /// </summary>
        /// <param name="card">La carte à bouger</param>
        /// <param name="newPosition">La position à prendre</param>
        /// <exception cref="NotInPileException">Si la carte n'est pas dans la pile</exception>
        internal bool MoveInternal(Card card, int newPosition)
        {
            return MoveTo(this, card, newPosition);
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}[{string.Join(", ", _cardList)}]";
        }
    }
}