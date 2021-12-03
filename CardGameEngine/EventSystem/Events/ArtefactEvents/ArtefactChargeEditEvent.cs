using CardGameEngine.Cards;

namespace CardGameEngine.EventSystem.Events.ArtefactEvents
{
    public class ArtefactChargeEditEvent : ArtefactEvent, IPropertyChangeEvent<Artefact, int>
    {
        public int NewChargeCount { get; set; }

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