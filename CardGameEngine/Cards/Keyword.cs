namespace CardGameEngine.Cards
{
    public class Keyword
    {
        public string Name { get; }

        public string EffectId { get; }


        public bool DoEffect(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}