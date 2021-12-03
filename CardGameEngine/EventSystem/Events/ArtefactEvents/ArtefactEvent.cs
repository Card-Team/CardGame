using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.ArtefactEvents
{
    public abstract class ArtefactEvent : CancellableEvent
    {
        public Artefact Artefact { get; set; }
    }
}