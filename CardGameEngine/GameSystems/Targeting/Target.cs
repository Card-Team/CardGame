using CardGameEngine.Cards;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Targeting
{
    /// <summary>
    /// Classe représentant une cible
    /// </summary>
    /// <seealso cref="TargetType"/>
    public class Target
    {
        /// <summary>
        /// Type de la cible
        /// </summary>
        /// <seealso cref="TargetTypes"/>
        public TargetTypes TargetType { get; }

        /// <summary>
        /// Choix de la cible manuel ou non
        /// </summary>
        public bool IsAutomatic { get; }

        /// <summary>
        /// Nom de la cible
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Prédicat indiquant si une carte peut être ciblée
        /// </summary>
        private readonly Closure? _cardFilter;

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="name">Nom de la cible</param>
        /// <param name="targetType">Type de la cible</param>
        /// <param name="isAutomatic">Choix de la cible manuel ou non</param>
        /// <param name="cardFilter">Prédicat indiquant si une carte peut être ciblée</param>
        public Target(string name, TargetTypes targetType, bool isAutomatic, Closure? cardFilter = null)
        {
            Name = name;
            TargetType = targetType;
            IsAutomatic = isAutomatic;
            _cardFilter = cardFilter;
        }

        /// <summary>
        /// Méthode indiquant si la carte donnée est conforme au filtre
        /// </summary>
        /// <param name="card">La carte à tester</param>
        /// <returns>Une booléen en fonction de la validité</returns>
        public bool IsValidTarget(Card card)
        {
            return _cardFilter == null || _cardFilter.Call(card).Boolean;
        }
    }
}