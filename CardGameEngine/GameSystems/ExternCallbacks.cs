using System.Collections.Generic;
using CardGameEngine.Cards;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems
{
    /// <summary>
    /// Interface que le moteur utilise pour interagir avec l'application externe
    /// </summary>
    public interface IExternCallbacks
    {
        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut demander à l'application externe de cibler une carte
        /// </summary>
        /// <param name="effectOwner"></param>
        /// <param name="targetName">Une description de la cible demandée</param>
        /// <param name="cardList">La liste des cibles valides</param>
        /// <returns>Une cible parmi la liste</returns>
        public Card ExternCardAskForTarget(Player effectOwner, string targetName, List<Card> cardList);

        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut demander a l'application externe de cibler un joueur
        /// </summary>
        /// <param name="effectOwner"></param>
        /// <param name="targetName">Une description de la cible demandée</param>
        /// <returns>Le joueur ciblé</returns>
        public Player ExternPlayerAskForTarget(Player effectOwner, string targetName);

        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut demander à l'application externe de montrer une carte a un joueur
        /// </summary>
        ///<param name="player">Le joueur auquel montrer la carte</param>
        ///<param name="card">La carte a montrer</param>
        /// <returns>La cible demandée</returns>
        public void ExternShowCard(Player player, Card card);

        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut demander à l'application externe de faire choisir une carte parmis une liste a un joueur
        /// </summary>
        ///<param name="player">Le joueur auquel demander</param>
        ///<param name="card">La liste parmis la quelle choisir</param>
        /// <returns>Un choix</returns>
        public Card ExternChooseBetween(Player player, List<Card> card);
        
        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut signaler à l'application externe que la partie s'est terminée
        /// </summary>
        /// <param name="winner">Le joueur qui a gagné la partie</param>
        public void ExternGameEnded(Player winner);


        /// <summary>
        /// Délégué appelé lorsque un script affiche du texte avec print()
        /// </summary>
        /// <param name="source">Le script source</param>
        /// <param name="debugPrint">Le texte</param>
        public void DebugPrint(string from,string source, string debugPrint);
    }
}