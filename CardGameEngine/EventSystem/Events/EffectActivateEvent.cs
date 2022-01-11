using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Évènement représentant l'activation d'un effet
    /// </summary>
    internal class EffectActivateEvent : Event
    {
        /// <summary>
        /// L'effet activé
        /// </summary>
        public Effect Effect { get; }

        public EffectActivateEvent(Effect effect)
        {
            Effect = effect;
        }
    }
}