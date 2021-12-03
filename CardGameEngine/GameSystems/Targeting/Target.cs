namespace CardGameEngine.GameSystems.Targeting
{
    public abstract class Target
    {
        public TargetTypes TargetType { get; }

        public bool IsAutomatic;

        public string Name;
    }
}