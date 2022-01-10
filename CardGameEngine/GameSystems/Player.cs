using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using CardGameEngine.Cards;
using CardGameEngine.Cards.CardPiles;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine.GameSystems
{
    /// <summary>
    /// Classe représentant un joueur
    /// </summary>
    public class Player : ITargetable
    {
        /// <summary>
        /// Nom du joueur
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Pioche du joueur
        /// </summary>
        public CardPile Deck { get; }

        /// <summary>
        /// Main du joueur
        /// </summary>
        public CardPile Hand { get; set; }

        /// <summary>
        /// Défausse du joueur
        /// </summary>
        public DiscardPile Discard;

        /// <summary>
        /// Référence à la partie
        /// </summary>
        private readonly Game _game;

        /// <summary>
        /// Les 2 artefacts du joueurs
        /// </summary>
        public Artefact[] Artefacts { get; } = new Artefact[2];

        /// <summary>
        /// Points d'action du joueur
        /// </summary>
        public EventProperty<Player, int, ActionPointsEditEvent> ActionPoints { get; }

        /// <summary>
        /// Points d'action maximum du joueur
        /// </summary>
        public EventProperty<Player, int, MaxActionPointsEditEvent> MaxActionPoints { get; }

        /// <summary>
        /// L'autre joueur
        /// </summary>
        public Player OtherPlayer => this == _game.Player1 ? _game.Player2 : _game.Player1;

        public IEnumerable<Card> Cards => Hand.Concat(Deck).Concat(Discard);


        public Player(Game game, string name, List<Card> cards)
        {
            _game = game;
            Name = name;
            Deck = new CardPile(game.EventManager, cards);
            ActionPoints = new EventProperty<Player, int, ActionPointsEditEvent>(this, game.EventManager, 0);
            MaxActionPoints = new EventProperty<Player, int, MaxActionPointsEditEvent>(this, game.EventManager, Game.DefaultMaxActionPoint);
        }


        /// <summary>
        /// Piocher une carte
        /// </summary>
        /// <param name="card">La carte à piocher</param>
        internal void DrawCard()
        {
            if (Deck.Count > 0)
            {
                Deck.MoveTo(Hand, Deck[0], Hand.Count);
            }
        }

        /// <summary>
        /// Transfère toutes les cartes de la défausse dans la pioche
        /// </summary>
        /// <seealso cref="DiscardPile"/>
        /// <seealso cref="CardPile"/>
        public void LoopDeck()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Déplace une carte vers la défausse
        /// </summary>
        /// <param name="card">La carte à défausser</param>
        /// <seealso cref="DiscardPile"/>
        public void DiscardCard(Card card)
        {
            throw new System.NotImplementedException();
        }
    }
}