using System.Collections.Generic;
using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine.GameSystems
{
    public class Effect
    {
        public EffectType EffectType { get; }

        public int EffectId { get; }

        public List<Target> AllTargets { get; }

        public bool DoEffect(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}