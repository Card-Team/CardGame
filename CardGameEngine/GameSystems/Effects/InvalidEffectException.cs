using System;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Exception lancée lorsqu'un script d'effet est invalide
    /// </summary>
    public class InvalidEffectException : Exception
    {
        internal InvalidEffectException(string effectId, EffectType effectType, string effectContent,
            string? message = "") :
            base($"L'effet {effectId} de type {effectType} est invalide{(message != "" ? " : " : "")}{message}")
        {
            EffectContent = effectContent;
        }

        internal InvalidEffectException(string effectId, EffectType effectType, InterpreterException exc,
            string effectContent) :
            base($"L'effet {effectId} de type {effectType} est invalide")
        {
            InnerException = exc;
            EffectContent = effectContent;
        }

        public new InterpreterException? InnerException { get; }
        public string EffectContent { get; }

        internal InvalidEffectException(string message, string effectContent) :
            base(message)
        {
            EffectContent = effectContent;
        }

        public InvalidEffectException(Effect effectId, string excMessage, string effectContent) :
            this(effectId.EffectId, effectId.EffectType, effectContent, excMessage)

        {
        }

        public InvalidEffectException(string message) : base(message)
        {
        }
    }
}