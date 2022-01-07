using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    /// <summary>
    /// Évènement correspondant au bouclage du deck d'un joueur
    /// </summary>
    public class DeckLoopEvent : Event
    {
        /// <summary>
        /// Joueur en train de jouer
        /// </summary>
        public Player Player { get; internal set; }
    }
}