using System;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Exception lancée lorsqu'un script d'effet est invalide
    /// </summary>
    public class InvalidEffectException : Exception
    {
        internal InvalidEffectException(string effectId, EffectType effectType, string? message = "") :
            base($"L'effet {effectId} de type {effectType} est invalide{(message != "" ? " : " : "")}{message}")
        {
        }

        internal InvalidEffectException(string effectId, EffectType effectType, InterpreterException exc) :
            base($"L'effet {effectId} de type {effectType} est invalide")
        {
            InnerException = exc;
        }

        public new InterpreterException? InnerException { get; }

        internal InvalidEffectException(string message) :
            base(message)
        {
        }

        public InvalidEffectException(Effect effectId, string excMessage) :
            this(effectId.EffectId, effectId.EffectType, excMessage)

        {
        }
    }
}