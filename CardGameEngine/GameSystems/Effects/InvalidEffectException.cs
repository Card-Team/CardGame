﻿using System;

namespace CardGameEngine.GameSystems.Effects
{
    public class InvalidEffectException : Exception
    {
        private readonly string _effectId;
        private readonly EffectType _effectType;

        public InvalidEffectException(string effectId, EffectType effectType) :
            base($"L'effet {effectId} de type {effectType} est invalide")
        {
            _effectId = effectId;
            _effectType = effectType;
        }
    }
}