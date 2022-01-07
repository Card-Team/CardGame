using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Évènement annulable correspondant au changement du nombre de points d'action d'un joueur
    /// </summary>
    public class ActionPointEditEvent : CancellableEvent, IPropertyChangeEvent<Player, int>
    {
        /// <summary>
        /// Le joueur touché
        /// </summary>
        public Player Player { get; internal set; }

        /// <summary>
        /// Ancien nombre de points
        /// </summary>
        public int OldPointCount { get; private set; }

        /// <summary>
        /// Nouveau nombre de points
        /// </summary>
        public int NewPointCount { get; internal set; }


        Player IPropertyChangeEvent<Player, int>.Sender
        {
            get => Player;
            set => Player = value;
        }

        int IPropertyChangeEvent<Player, int>.NewValue
        {
            get => NewPointCount;
            set => NewPointCount = value;
        }

        int IPropertyChangeEvent<Player, int>.OldValue
        {
            get => OldPointCount;
            set => OldPointCount = value;
        }
    }
}