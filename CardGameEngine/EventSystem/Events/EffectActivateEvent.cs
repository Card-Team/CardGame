using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Évènement représentant l'activation d'un effet
    /// </summary>
    internal class EffectActivateEvent : CancellableEvent
    {
        /// <summary>
        /// L'effet activé
        /// </summary>
        internal Effect Effect { get; set; }
    }
}