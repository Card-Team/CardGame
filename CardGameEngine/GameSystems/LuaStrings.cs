namespace CardGameEngine.GameSystems
{
    internal static class LuaStrings
    {
        public static class Card
        {
            public const string NameProperty = "name";
            public const string CostProperty = "pa_cost";
            public const string MaxLevelProperty = "max_level";
            public const string DescriptionProperty = "description";
            public const string TargetsProperty = "targets";
            public const string ImageIdProperty = "image_id";
            public const string ChainModeProperty = "chain_mode";

            public const string PreconditionMethod = "precondition";
            public const string DoEffectMethod = "do_effect";

            public const string OnLevelChangeMethod = "on_level_change";
            public const string OnCardCreateMethod = "on_card_create";
        }

        public static class Artefact
        {
        }

        public static class Keyword
        {
        }
    }
}