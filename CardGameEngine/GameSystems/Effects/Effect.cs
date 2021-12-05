using System.Collections.Generic;
using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine.GameSystems.Effects
{
    public class Effect
    {
        public EffectType EffectType { get; }

        public string EffectId { get; }

        public List<Target> AllTargets { get; }


        public Effect(EffectType effectType, string effectId, List<Target> targets)
        {
            EffectType = effectType;
            EffectId = effectId;
            AllTargets = targets;
        }

        public bool DoEffect(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}