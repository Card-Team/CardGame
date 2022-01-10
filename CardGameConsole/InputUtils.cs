using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;

namespace CardGameConsole
{
    public static class InputUtils
    {
        public static Card ChooseFrom(List<Card> list)
        {
            Console.WriteLine("Veuillez sélectionner une carte parmi la liste suivante : ");

            foreach (var (card, i) in list.Select((card, i) => (card, i)))
            {
                Console.WriteLine($"{i} - {card}");
            }
        }
    }
}