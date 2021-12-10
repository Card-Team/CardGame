using CardGameEngine.Cards;
using CardGameEngine.Cards.CardPiles;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;

namespace CardGameEngine.GameSystems
{
    /// <summary>
    /// Classe représentant un joueur
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Nom du joueur
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Pioche du joueur
        /// </summary>
        public CardPile Deck { get; set; }

        /// <summary>
        /// Main du joueur
        /// </summary>
        public CardPile Hand { get; set; }

        /// <summary>
        /// Défausse du joueur
        /// </summary>
        public DiscardPile Discard;

        /// <summary>
        /// Les 2 artefacts du joueurs
        /// </summary>
        public Artefact[] Artefacts { get; } = new Artefact[2];

        /// <summary>
        /// Points d'action du joueur
        /// </summary>
        public EventProperty<Player, int, ActionPointEditEvent> ActionPoints { get; }


        /// <summary>
        /// Piocher une carte
        /// </summary>
        /// <param name="card">La carte à piocher</param>
        public void DrawCard(Card? card = null)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Déplace une carte vers la défausse en mode upgrade
        /// </summary>
        /// <param name="card">La carte à défausser</param>
        public void PrepareCardUpgrade(Card card)
        {
            throw new System.NotImplementedException();
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