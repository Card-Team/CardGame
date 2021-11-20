using CardGameEngine;
using CardGameEngine.Cards.CardPiles;
using Dynamic;

namespace GameSystems
{
    public class Player
    {
        public CardPile Deck;

        public string Name { get; }

        public CardPile Hand;

        public Artefact[] Artefacts { get; } = new Artefact[2];

        public DynamicValue<int> ActionPoints { get; }

        public DiscardPile Discard;


        public void DrawCard()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void PrepareCardUpgrade(Card card)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void LoopDeck()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void DiscardCard(Card card)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }
}