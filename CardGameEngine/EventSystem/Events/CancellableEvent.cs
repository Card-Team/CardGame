using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Classe abstraite représentant les évènement pouvant être annulés
    /// </summary>
    public abstract class CancellableEvent : Event
    {
        /// <summary>
        /// Booléen indiquant l'annulation de l'évènement
        /// </summary>
        [MoonSharpVisible(true)]
        public bool Cancelled { get; internal set; }
    }
}