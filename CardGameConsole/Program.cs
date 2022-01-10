using System;
using System.IO;
using System.Linq;
using CardGameEngine;

namespace CardGameConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("\nLancement d'une partie : ...\n");

            // Joueur 1
            Console.Write("Joueur 1, veuillez entrer votre nom : ");
            var name1 = Console.ReadLine();

            Console.WriteLine($"\nBonjour, {name1}");
            Console.WriteLine("Veuillez m'indiquer le fichier contenant vos cartes (CardGameConsole/Decks/) :");
            var deck1 = File.ReadLines("../../Decks/" + Console.ReadLine()).ToList();

            // Joueur 2
            Console.Write("\nJoueur 2, veuillez entrer votre nom : ");
            var name2 = Console.ReadLine();

            Console.WriteLine($"\nBonjour, {name2}");
            Console.WriteLine("Veuillez m'indiquer le fichier contenant vos cartes (CardGameConsole/Decks/) :");
            var deck2 = File.ReadLines("../../Decks/" + Console.ReadLine()).ToList();

            var game = new Game("../../../CardGameEngine/EffectsScripts/", new ConsoleExternCallbacks(), deck1, deck2);

            Console.WriteLine(game.Player1.Deck.ToString());
            Console.WriteLine(game.Player2.Deck.ToString());
        }
    }
}