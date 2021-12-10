using System;
using CardGameEngine.Cards;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.GameSystems;
using CardGameEngine.GameSystems.Effects;

namespace CardGameEngine
{
    /// <summary>
    /// Classe principale du projet représentant une partie
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Le joueur en train de jouer
        /// </summary>
        public Player CurrentPlayerTurn { get; }

        /// <summary>
        /// Le joueur 1
        /// </summary>
        public Player Player1 { get; }

        /// <summary>
        /// Le joueur 2
        /// </summary>
        public Player Player2 { get; }

        /// <summary>
        /// Gestionnaire des évènements de la partie
        /// </summary>
        public EventManager EventManager { get; }

        /// <summary>
        /// Base de données de tous les effets
        /// </summary>
        /// <seealso cref="Effect"/>
        public EffectsDatabase EffectsDatabase { get; }


        /// <summary>
        /// Teste si un joueur a gagné la partie
        /// </summary>
        /// <param name="playerToCheck">La joueur à tester</param>
        /// <returns>True si le joueur a gagné</returns>
        public bool CheckHasWon(Player playerToCheck)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lance le tour d'un joueur
        /// </summary>
        /// <param name="player">Le joueur qui doit jouer</param>
        public void StartPlayerTurn(Player player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Termine le tour du joueur actuel
        /// </summary>
        public void EndPlayerTurn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Un joueur utilise une carte
        /// </summary>
        /// <param name="player">Le joueur</param>
        /// <param name="card">La carte jouée</param>
        public void PlayCard(Player player, Card card)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Un joueur active un artefact
        /// </summary>
        /// <param name="player">Le joueur</param>
        /// <param name="artefact">L'artefact activé</param>
        public void ActivateArtifact(Player player, Artefact artefact)
        {
            throw new NotImplementedException();
        }
    }
}