using System.Collections.Generic;
using CardGameEngine.GameSystems;
using Dynamic;

namespace CardGameEngine
{
    public class Card
    {
        public DynamicValue<string> Name { get; }

        public int MaxLevel { get; }

        public DynamicValue<string> Cost { get; }

        public List<Keyword> Keywords;


        public bool CanBePlayedIn(Game game)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public Effect GetEffect()
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }
}