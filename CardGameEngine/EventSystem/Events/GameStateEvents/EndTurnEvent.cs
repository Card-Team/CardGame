using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    /// <summary>
    /// Évènement correspondant à la fin du tour d'un joueur
    /// </summary>
    public class EndTurnEvent : Event
    {
        /// <summary>
        /// Joueur en train de jouer
        /// </summary>
        public Player Player { get; internal set; }
    }
}