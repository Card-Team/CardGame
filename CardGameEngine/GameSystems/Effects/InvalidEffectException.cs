using System;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Exception lancée lorsqu'un script d'effet est invalide
    /// </summary>
    public class InvalidEffectException : Exception
    {
        public InvalidEffectException(string effectId, EffectType effectType) :
            base($"L'effet {effectId} de type {effectType} est invalide")
        {
        }
    }
}