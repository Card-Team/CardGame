using System;

namespace CardGameEngine.Cards.CardPiles
{
    /// <summary>
    /// Exception lancée lorsque d'une carte essaye d'être sortie d'une pile à laquelle elle n'appartient pas
    /// </summary>
    public class NotInPileException : Exception
    {
        internal NotInPileException(Card card) :
            base($"La carte {card.Name} n'est pas dans la pile")
        {
        }
    }
}