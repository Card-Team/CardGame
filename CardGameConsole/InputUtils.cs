using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;

namespace CardGameConsole
{
    public static class InputUtils
    {
        public static Card ChooseFrom(Player pov, IEnumerable<Card> list)
        {
            Console.WriteLine("Veuillez sélectionner une carte parmi la liste suivante : ");


            var splitted = list.SplitIntoPiles().ToList();
            var flattened = splitted.SelectMany(s => s).ToList();
            var index = 0;
            foreach (var pile in splitted)
            {
                if (pile.Key != PileIdentifier.Unknown)
                    Console.WriteLine($"{pile.Key} : ");
                foreach (var (card, visible) in pile.WithVisionInfo(pov))
                {
                    Console.WriteLine($"{index} - {(visible ? card.ToString() : "Inconnu")}");
                    index++;
                }
            }
            
            return ChooseList(flattened);
        }


        public static T ChooseList<T>(List<(string,T)> elements) where T: class
        {
            for (var i = 0; i < elements.Count; i++)
            {
                Console.WriteLine($" - {elements[i].Item1}");
            }

            return ChooseFromList(elements.Select(s => s.Item2).ToList());
        }
        
        public static T ChooseList<T>(List<T> elements) where T: class
        {
            for (var i = 0; i < elements.Count; i++)
            {
                Console.WriteLine($" - {elements[i]}");
            }

            return ChooseFromList(elements);
        }
        public static T ChooseFromList<T>(List<T> elements) where T: class
        {

            T? chosen = null;
            do
            {
                Console.WriteLine("Choix : ");
                if (!int.TryParse(Console.ReadLine()?.Trim(), out var result)) continue;
                
                if (result >= 0 && result < elements.Count)
                {
                    chosen = elements[result];
                }
            } while (chosen == null);

            return chosen;
        }
    }
}