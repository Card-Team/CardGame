using CardGameEngine.Cards.CardPiles;

namespace CardGameEngine.EventSystem.Events.CardEvents
{
    /// <summary>
    /// Évènement annulable correspondant au changement de pile d'une carte
    /// </summary>
    public class CardMovePileEvent : CardEvent
    {
        /// <summary>
        /// Pile d'origine
        /// </summary>
        public CardPile SourcePile { get; set; }

        /// <summary>
        /// Position dans la pile d'origine
        /// </summary>
        public int SourceIndex { get; set; }

        /// <summary>
        /// Pile de destination
        /// </summary>
        public CardPile DestPile { get; set; }

        /// <summary>
        /// Position dans la pile de destination
        /// </summary>
        public int DestIndex { get; set; }
    }
}