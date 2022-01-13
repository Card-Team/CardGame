using CardGameEngine.Cards;
using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    ///     Évènement annulable correspondant à l'activation d'un effet de carte
    /// </summary>
    public class CardEffectPlayEvent : TransferrableCardEvent
    {
        public Player WhoPlayed { get; internal set; }

        internal CardEffectPlayEvent(Player whoPlayed, Card card) : base(card)
        {
            WhoPlayed = whoPlayed;
        }
    }
}