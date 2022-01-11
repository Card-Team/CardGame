using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;
using Spectre.Console;

namespace CardGameConsole
{
    public static class InputUtils
    {
        public static Card? ChooseFrom(Player pov, IEnumerable<Card> list, bool annulable = false)
        {

            var split = list.SplitIntoPiles().ToList();
            var flattened = new Dictionary<string, Card>();

            var index = 0;

            var prompt = new SelectionPrompt<string>();

            prompt.Title("Veuillez sélectionner une carte");

            foreach (var pile in split)
            {
                var builded = new List<string>();
                foreach (var (card, visible) in pile.WithVisionInfo())
                {
                    var text = $"{index} - {(visible ? card.ToString() : "Inconnu")}";
                    builded.Add(text);
                    flattened[text] = card;
                    index++;
                }


                prompt.AddChoiceGroup(pile.Key.Display(),
                    builded);
            }

            if (annulable)
            {
                var retour = "⬅ Retour";
                prompt.AddChoice(retour);
                flattened[retour] = null!;
            }

            var res = AnsiConsole.Prompt(prompt);

            return flattened[res];
        }


        public static T ChooseList<T>(string title, Dictionary<string, T> elements)
        {
            var result = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(title)
                    .Mode(SelectionMode.Leaf)
                    .AddChoices(elements.Keys));

            return elements[result];
        }


        public static T ChooseList<T>(string title, IEnumerable<T> elements)
        {
            return ChooseList(title,elements.ToDictionary(e => e?.ToString() ?? "null", e => e));
        }
    }
}