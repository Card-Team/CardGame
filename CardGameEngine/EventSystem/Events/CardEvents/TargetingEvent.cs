using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement correspondant à un ciblage
    /// </summary>
    public class TargetingEvent : Event
    {
        /// <summary>
        /// La cible enregistrée
        /// </summary>
        public Target TargetData { get; internal set; }

        public ITargetable ResolvedTarget { get; internal set; }
    }
}