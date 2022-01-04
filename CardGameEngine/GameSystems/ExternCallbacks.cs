using CardGameEngine.Cards;
using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine.GameSystems
{
    /// <summary>
    /// Interface que le moteur utilise pour interagir avec l'application externe
    /// </summary>
    public interface IExternCallbacks
    {
        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut demander à l'application externe de choisir une cible
        /// </summary>
        /// <param name="target">Une description de la cible demandée</param>
        /// <returns>La cible demandée</returns>
        public T ExternAskForTarget<T>(Target target);

        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut demander à l'application externe de montrer une carte a un joueur
        /// </summary>
        ///<param name="player">Le joueur auquel montrer la carte</param>
        ///<param name="card">La carte a montrer</param>
        /// <returns>La cible demandée</returns>
        public void ExternShowCard(Player player, Card card);

        /// <summary>
        /// Délégué appelé lorsque le moteur du jeu veut signaler à l'application externe que la partie s'est terminée
        /// </summary>
        /// <param name="winner">Le joueur qui a gagné la partie</param>
        public void ExternGameEnded(Player winner);
    }
}