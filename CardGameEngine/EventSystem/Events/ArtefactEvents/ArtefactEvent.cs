using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.ArtefactEvents
{
    /// <summary>
    /// Classe abstraite pour les évènements liés à un artefact 
    /// </summary>
    public abstract class ArtefactEvent : CancellableEvent
    {
        /// <summary>
        /// L'artefact concerné
        /// </summary>
        public Artefact Artefact { get; internal set; }

        protected ArtefactEvent(Artefact artefact)
        {
            Artefact = artefact;
        }
    }
}