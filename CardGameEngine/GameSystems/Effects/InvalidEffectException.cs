using System;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Exception lancée lorsqu'un script d'effet est invalide
    /// </summary>
    internal class InvalidEffectException : Exception
    {
        internal InvalidEffectException(string effectId, EffectType effectType) :
            base($"L'effet {effectId} de type {effectType} est invalide")
        {
        }
        
        internal InvalidEffectException(string message) :
            base(message)
        {
        }
    }
}