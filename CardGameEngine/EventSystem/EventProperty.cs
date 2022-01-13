using CardGameEngine.Cards;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.EventSystem
{
    /// <summary>
    /// Propriété devant déclencher un évènement à chaque modification
    /// </summary>
    /// <typeparam name="S">Type de l'objet (Artefact/Card/Keyword)</typeparam>
    /// <typeparam name="T">Type de la valeur (int, string, ...)</typeparam>
    /// <typeparam name="ET">IPropertyChangeEvent</typeparam>
    /// <seealso cref="IPropertyChangeEvent{S,T}"/>
    public class EventProperty<S, T, ET> where ET : CancellableEvent, IPropertyChangeEvent<S, T>, new()
    {
        /// <summary>
        /// Objet lié à l'évènement
        /// </summary>
        protected S _sender;

        /// <summary>
        /// Valeur de la propriété
        /// </summary>
        public T Value { get; private set; }

        private EventManager EvtManager;
        private readonly bool _isInevitable;


        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="sender">L'objet lié à l'évènement</param>
        /// <param name="evtManager"></param>
        /// <param name="value">Valeur par défaut</param>
        /// <param name="isInevitable">Suis-je inéluctable ?</param>
        internal EventProperty(S sender, EventManager evtManager, T value, bool isInevitable = false)
        {
            _sender = sender;
            EvtManager = evtManager;
            _isInevitable = isInevitable;
            Value = value;
        }

        /// <summary>
        /// Essaye de changer la valeur de la propriété par la nouvelle entrée
        /// </summary>
        /// <param name="newVal">La nouvelle valeur</param>
        [MoonSharpVisible(true)]
        internal virtual T TryChangeValue(T newVal)
        {
            var evt = new ET
            {
                Sender = _sender,
                OldValue = Value,
                NewValue = newVal
            };

            using (var postEvent = EvtManager.SendEvent(evt))
            {
                if (!_isInevitable && postEvent.Event.Cancelled)
                    return Value;
                StealthChange(_isInevitable ? newVal : postEvent.Event.NewValue);
            }

            return Value;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "null";
        }

        /// <summary>
        ///     Modifie la propriété sans déclencher d'évenement
        /// </summary>
        public void StealthChange(T value)
        {
            Value = value;
        }
    }

    class LevelEventProperty : EventProperty<Card, int, CardLevelChangeEvent>
    {
        internal LevelEventProperty(Card sender, EventManager evtManager, int value, bool isInevitable = false) : base(
            sender, evtManager, value, isInevitable)
        {
        }

        internal override int TryChangeValue(int newVal)
        {
            var oldLevel = Value;
            base.TryChangeValue(newVal);
            _sender.OnLevelChange(oldLevel, Value);
            return Value;
        }
    }
}