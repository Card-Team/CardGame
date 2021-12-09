using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.ArtefactEvents;

namespace CardGameEngine.Cards
{
    public class Artefact
    {
        public string Name { get; }

        public int MaxCharge { get; }

        public EventProperty<Artefact, int, ArtefactChargeEditEvent> CurrentCharge { get; }

        public int EffectId { get; }

        public List<Keyword> Keywords { get; set; }


        public bool DoEffect(Game game)
        {
            throw new System.NotImplementedException();
        }

        public bool CanBeActivated()
        {
            throw new System.NotImplementedException();
        }
    }
}