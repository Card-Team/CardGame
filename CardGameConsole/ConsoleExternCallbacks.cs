using System;
using System.Collections.Generic;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;

namespace CardGameConsole
{
    public class ConsoleExternCallbacks : IExternCallbacks
    {
        public Card ExternCardAskForTarget(Player effectOwner, string targetName, List<Card> cardList)
        {
            Console.WriteLine($"Choisissez une carte pour {targetName}");
            
        }

        public Player ExternPlayerAskForTarget(Player effectOwner, string targetName)
        {
            throw new System.NotImplementedException();
        }

        public void ExternShowCard(Player player, Card card)
        {
            throw new System.NotImplementedException();
        }

        public Card ExternChooseBetween(Player player, List<Card> card)
        {
            throw new System.NotImplementedException();
        }

        public void ExternGameEnded(Player winner)
        {
            throw new System.NotImplementedException();
        }
    }
}