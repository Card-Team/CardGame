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


            var split = list.SplitIntoPiles().ToList();
            var flattened = split.SelectMany(s => s).ToList();
            var index = 0;
            foreach (var pile in split)
            {
                if (pile.Key != PileIdentifier.Unknown)
                    Console.WriteLine($"{pile.Key.Display()} : ");
                foreach (var (card, visible) in pile.WithVisionInfo(pov))
                {
                    Console.WriteLine($"{index} - {(visible ? card.ToString() : "Inconnu")}");
                    index++;
                }
            }

            return ChooseFromList(flattened);
        }


        public static T ChooseList<T>(List<(string, T)> elements) where T : class
        {
            for (var i = 0; i < elements.Count; i++)
            {
                Console.WriteLine($" - {elements[i].Item1}");
            }

            return ChooseFromList(elements.Select(s => s.Item2).ToList());
        }

        public static T ChooseList<T>(List<T> elements) where T : class
        {
            foreach (var elem in elements)
            {
                Console.WriteLine($" - {elem}");
            }

            return ChooseFromList(elements);
        }

        public static T ChooseFromList<T>(List<T> elements)
        {
            var chosen = false;
            T res = default;
            do
            {
                Console.Write("Choix : ");
                if (!int.TryParse(Console.ReadLine()?.Trim(), out var result)) continue;

                if (result < 0 || result >= elements.Count) continue;
                res = elements[result];
                chosen = true;
            } while (!chosen);

            return res ?? throw new InvalidOperationException();
        }
    }
}