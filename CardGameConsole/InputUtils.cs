using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.GameSystems;
using Spectre.Console;

namespace CardGameConsole
{
    public static class InputUtils
    {
        public static Card? ChooseFrom(Player pov, IEnumerable<Card> list, bool annulable = false,
            bool splitCards = true)
        {
            var split = list.SplitIntoPiles().ToList();
            var flattened = new Dictionary<string, Card>();

            var index = 0;

            var prompt = new SelectionPrompt<string>();

            prompt.Title("Veuillez sélectionner une carte");

            foreach (var pile in split)
            {
                var built = new List<string>();
                foreach (var (card, visible) in pile.WithVisionInfo(pov))
                {
                    var text = $"{index} - {(visible || !splitCards ? card.ToString() : "Inconnu")}";
                    built.Add(text);
                    flattened[text] = card;
                    index++;
                }


                if (splitCards)
                    prompt.AddChoiceGroup(pile.Key.Display(pov),
                        built);
                else
                    prompt.AddChoices(built);
            }

            if (annulable)
            {
                var retour = Emoji.Known.LeftArrow + " Retour";
                prompt.AddChoice(Emoji.Replace(retour));
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
    }
}