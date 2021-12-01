using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Cards.CardPiles
{
    public class CardPile : IEnumerable<Card>
    {
        private List<Card> cardList;


        //Permet de faire pile[i] comme si c'était un tableau normal
        //Oui on peut changer ça en c#. Et les opérateurs aussi, genre + et -
        public Card this[int i] => cardList.ElementAt(i);

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void MoveTo(CardPile currentCardPile, Card card)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }
}