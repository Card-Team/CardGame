using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Évènement représentant l'activation d'un effet
    /// </summary>
    public class EffectActivateEvent : CancellableEvent
    {
        /// <summary>
        /// L'effet activé
        /// </summary>
        public Effect Effect { get; set; }
    }
}