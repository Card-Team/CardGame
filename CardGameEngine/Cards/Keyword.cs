using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine.Cards
{
    /// <summary>
    /// Classe représentant un mot clé
    /// </summary>
    public class Keyword
    {
        /// <summary>
        /// Nom du mot clé
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Effet du mot clé
        /// </summary>
        public Effect Effect { get; }
        
    }
}