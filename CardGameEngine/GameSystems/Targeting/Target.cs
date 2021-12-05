using CardGameEngine.Cards;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Targeting
{
    public class Target
    {
        public TargetTypes TargetType { get; }

        public bool IsAutomatic { get; }

        public string Name { get; }

        private readonly Closure? _cardFilter;

        public Target(string name, TargetTypes targetType, bool isAutomatic, Closure? cardFilter = null)
        {
            Name = name;
            TargetType = targetType;
            IsAutomatic = isAutomatic;
            _cardFilter = cardFilter;
        }

        public bool IsValidTarget(Card card)
        {
            return _cardFilter == null || _cardFilter.Call(card).Boolean;
        }
    }
}