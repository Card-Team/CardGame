using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    /// <summary>
    /// Évènement correspondant au début du tour d'un joueur
    /// </summary>
    public class StartTurnEvent : Event
    {
        /// <summary>
        /// Joueur en train de jouer
        /// </summary>
        public Player Player { get; internal set; }

        internal StartTurnEvent(Player player)
        {
            Player = player;
        }
    }
}