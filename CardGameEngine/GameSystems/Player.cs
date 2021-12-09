using CardGameEngine.Cards;
using CardGameEngine.Cards.CardPiles;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;

namespace CardGameEngine.GameSystems
{
    public class Player
    {
        public string Name { get; }

        public CardPile Deck { get; set; }

        public CardPile Hand { get; set; }

        public DiscardPile Discard;

        public Artefact[] Artefacts { get; } = new Artefact[2];

        public EventProperty<Player, int, ActionPointEditEvent> ActionPoints { get; }


        public void DrawCard(Card? theCard = null)
        {
            throw new System.NotImplementedException();
        }

        public void PrepareCardUpgrade(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void LoopDeck()
        {
            throw new System.NotImplementedException();
        }

        public void DiscardCard(Card card)
        {
            throw new System.NotImplementedException();
        }
    }
}