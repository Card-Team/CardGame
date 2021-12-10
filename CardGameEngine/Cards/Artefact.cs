using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.ArtefactEvents;
using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine.Cards
{
    /// <summary>
    /// Classe représentant un artefact
    /// </summary>
    public class Artefact
    {
        /// <summary>
        /// Nom de l'artefact
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Charge maximale de l'artefact
        /// </summary>
        public int MaxCharge { get; }

        /// <summary>
        /// Charge actuelle de l'artefact
        /// </summary>
        public EventProperty<Artefact, int, ArtefactChargeEditEvent> CurrentCharge { get; }

        /// <summary>
        /// Effet de l'artefact
        /// </summary>
        public Effect Effect { get; }


        /// <summary>
        /// Exécute l'effet de l'artefact
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
        public bool CanBeActivated(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}