using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.Cards.CardPiles;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.GameSystems
{
    /// <summary>
    /// Classe représentant un joueur
    /// </summary>
    public class Player : ITargetable
    {
        /// <summary>
        /// Pioche du joueur
        /// </summary>
        public CardPile Deck { get; }

        /// <summary>
        /// Main du joueur
        /// </summary>
        public CardPile Hand { get; }

        /// <summary>
        /// Défausse du joueur
        /// </summary>
        public DiscardPile Discard { get; }

        /// <summary>
        /// Référence à la partie
        /// </summary>
        private readonly Game _game;

        private readonly EventManager _eventManager;

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


        internal Player(Game game, List<Card> cards)
        {
            _game = game;
            _eventManager = game.EventManager;

            Deck = new CardPile(game, cards);
            Hand = new CardPile(game, 5);
            Discard = new DiscardPile(game);

            ActionPoints = new EventProperty<Player, int, ActionPointsEditEvent>(this, game.EventManager, 0);
            MaxActionPoints =
                new EventProperty<Player, int, MaxActionPointsEditEvent>(this, game.EventManager,
                    Game.DefaultMaxActionPoint);
        }


        /// <summary>
        /// Piocher une carte
        /// </summary>
        /// <param name="card">La carte à piocher</param>
        [MoonSharpVisible(true)]
        internal void DrawCard()
        {
            if (Deck.Count > 0)
            {
                Deck.MoveTo(Hand, Deck[0], Hand.Count);
            }
        }

        [MoonSharpVisible(true)]
        internal bool HasCard(Card card) => Cards.Contains(card);

        /// <summary>
        /// Transfère toutes les cartes de la défausse dans la pioche
        /// </summary>
        /// <seealso cref="DiscardPile"/>
        /// <seealso cref="CardPile"/>
        [MoonSharpVisible(true)]
        internal void LoopDeck()
        {
            var evt = new DeckLoopEvent(this);
            using (var post = _eventManager.SendEvent(evt))
            {
                var toLoop = post.Event.Player;
                foreach (var card in toLoop.Discard.ToList())
                {
                    if (toLoop.Discard.IsMarkedForUpgrade(card) && !card.IsMaxLevel)
                    {
                        card.Upgrade();
                    }

                    toLoop.Discard.MoveTo(toLoop.Deck, card, 0);
                }
            }
        }
    }
}