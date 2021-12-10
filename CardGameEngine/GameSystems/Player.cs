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
        /// <seealso cref="CardPile"/>
        public CardPile Deck { get; set; }

        /// <summary>
        /// Main du joueur
        /// </summary>
        /// <seealso cref="CardPile"/>
        public CardPile Hand { get; set; }

        /// <summary>
        /// Défausse du joueur
        /// </summary>
        /// <seealso cref="DiscardCard"/>
        public DiscardPile Discard;

        /// <summary>
        /// Les 2 artefacts du joueurs
        /// </summary>
        /// <seealso cref="Artefact"/>
        public Artefact[] Artefacts { get; } = new Artefact[2];

        /// <summary>
        /// Points d'action du joueur
        /// </summary>
        /// <seealso cref="EventProperty{S,T,ET}"/>
        public EventProperty<Player, int, ActionPointEditEvent> ActionPoints { get; }


        /// <summary>
        /// Piocher une carte
        /// </summary>
        /// <param name="card">La carte à piocher</param>
        /// <seealso cref="Card"/>
        public void DrawCard(Card? card = null)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Déplace une carte vers la défausse en mode upgrade
        /// </summary>
        /// <param name="card">La carte à défausser</param>
        /// <seealso cref="Card"/>
        public void PrepareCardUpgrade(Card card)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Transfère toutes les cartes de la défausse dans la pioche
        /// </summary>
        /// <seealso cref="CardPile"/>
        /// <seealso cref="DiscardPile"/>
        public void LoopDeck()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Déplace une carte vers la défausse
        /// </summary>
        /// <param name="card">La carte à défausser</param>
        public void DiscardCard(Card card)
        {
            throw new System.NotImplementedException();
        }
    }
}