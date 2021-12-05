using System.Collections.Generic;
using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine.GameSystems.Effects
{
    public class Effect
    {
        public EffectType EffectEffectType { get; }

        public int EffectEffectId { get; }

        public List<Target> AllTargets { get; }


        public Effect(EffectType effectType, int effectId, List<Target> targets)
        {
            EffectEffectType = effectType;
            EffectEffectId = effectId;
            AllTargets = targets;
        }

        public bool DoEffect(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}