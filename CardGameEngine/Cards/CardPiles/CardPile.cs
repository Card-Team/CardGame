using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Cards.CardPiles
{
    public class CardPile : IEnumerable<Card>
    {
        private List<Card> _cardList;

        public int Count => _cardList.Count;

        //Permet de faire pile[i] comme si c'était un tableau normal
        //Oui on peut changer ça en c#. Et les opérateurs aussi, genre + et -
        public Card this[int i] => _cardList.ElementAt(i);

        public bool Contains(Card card)
        {
            return _cardList.Contains(card);
        }

        public int IndexOf(Card card)
        {
            return _cardList.IndexOf(card);
        }

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public void MoveTo(CardPile newCardPile, Card card, int newPosition)
        {
            throw new System.NotImplementedException();
        }

        public void MoveInternal(Card card, int newPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}