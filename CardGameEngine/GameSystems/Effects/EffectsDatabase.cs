using System;
using System.Collections.Generic;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Effects
{
    public class EffectsDatabase
    {
        private Dictionary<int, Effect> _effectDictionary = new Dictionary<int, Effect>();

        public Effect this[int i] => _effectDictionary[i];

        private Effect LoadEffect(EffectType type, int effectId)
        {
            // Effect effect = new Effect(type, effectId, new List<Target>());
            // _effectDictionary.Add(effectId, effect);
            // return effect;
            throw new NotImplementedException();
        }

        public void LoadAllEffects(string path)
        {
            throw new NotImplementedException();
        }
    }
}