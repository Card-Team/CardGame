using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;
using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine.Cards
{
    /// <summary>
    /// Classe représentant une carte
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Nom de la carte
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Niveau maximum de la carte
        /// </summary>
        public int MaxLevel { get; }

        /// <summary>
        /// Coût de la carte (ActionPoints du player)
        /// </summary>
        public EventProperty<Card, int, CardCostChangeEvent> Cost { get; }

        /// <summary>
        /// Effet de la carte
        /// </summary>
        public Effect Effect { get; }

        /// <summary>
        /// Mots clé appliqués à la carte
        /// </summary>
        public List<Keyword> Keywords { get; set; }


        /// <summary>
        /// Exécute l'effet de la carte
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Un booléen en fonction de la réussite</returns>
        public bool DoEffect(Game game)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Vérifie la validité de la précondition de l'artefact
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Un booléen en fonction de la validité</returns>
        public bool CanBePlayed(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}