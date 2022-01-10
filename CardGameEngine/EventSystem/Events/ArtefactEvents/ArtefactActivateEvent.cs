using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.ArtefactEvents
{
    /// <summary>
    /// Évènement annulable correspondant à l'activation d'un artefact
    /// </summary>
    public class ArtefactActivateEvent : ArtefactEvent
    {
        internal ArtefactActivateEvent(Artefact artefact) : base(artefact)
        {
        }
    }
}