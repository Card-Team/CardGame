using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Évènement annulable correspondant au changement du nombre de points d'action d'un joueur
    /// </summary>
    public class MaxActionPointsEditEvent : CancellableEvent, IPropertyChangeEvent<Player, int>
    {
        /// <summary>
        /// Le joueur touché
        /// </summary>
        public Player Player { get; internal set; }

        /// <summary>
        /// Ancien nombre de points
        /// </summary>
        public int OldMaxPointCount { get; private set; }

        /// <summary>
        /// Nouveau nombre de points
        /// </summary>
        public int NewMaxPointCount { get; internal set; }


        Player IPropertyChangeEvent<Player, int>.Sender
        {
            get => Player;
            set => Player = value;
        }

        int IPropertyChangeEvent<Player, int>.NewValue
        {
            get => NewMaxPointCount;
            set => NewMaxPointCount = value;
        }

        int IPropertyChangeEvent<Player, int>.OldValue
        {
            get => OldMaxPointCount;
            set => OldMaxPointCount = value;
        }
    }
}