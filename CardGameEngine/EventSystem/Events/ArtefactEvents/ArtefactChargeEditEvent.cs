using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.ArtefactEvents
{
    /// <summary>
    /// Évènement annulable correspondant au changement du nombre de charges d'un artefact
    /// </summary>
    public class ArtefactChargeEditEvent : ArtefactEvent, IPropertyChangeEvent<Artefact, int>
    {
        /// <summary>
        /// Valeur de la nouvelle charge
        /// </summary>
        public int NewChargeCount { get; set; }

        /// <summary>
        /// Valeur de l'ancienne charge
        /// </summary>
        public int OldChargeCount { get; private set; }
        
        Artefact IPropertyChangeEvent<Artefact, int>.Sender
        {
            get => Artefact;
            set => Artefact = value;
        }

        int IPropertyChangeEvent<Artefact, int>.NewValue
        {
            get => NewChargeCount;
            set => NewChargeCount = value;
        }

        int IPropertyChangeEvent<Artefact, int>.OldValue
        {
            get => OldChargeCount;
            set => OldChargeCount = value;
        }
    }
}