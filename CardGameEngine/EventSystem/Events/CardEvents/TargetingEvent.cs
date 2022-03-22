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

        /// <summary>
        ///     La cible résolue
        /// </summary>
        public ITargetable ResolvedTarget { get; set; }

        internal TargetingEvent(Target targetData, ITargetable resolvedTarget)
        {
            TargetData = targetData;
            ResolvedTarget = resolvedTarget;
        }
    }
}