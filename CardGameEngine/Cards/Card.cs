using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;

namespace CardGameEngine.Cards
{
    public class Card
    {
        public string Name { get; }

        public int MaxLevel { get; }

        public EventProperty<Card, int, CardCostChangeEvent> Cost { get; }

        public EventProperty<Card, int, CardEffectIdChangeEvent> EffectId { get; }

        public List<Keyword> Keywords { get; set; }
    }
}