using System;
using System.IO;
using System.Linq;
using CardGameEngine;

namespace CardGameConsole
{
    internal class ConsoleGame
    {
        public static Game Game;

        public static string Player1Name;
        public static string Player2Name;
        
        public static void Main(string[] args)
        {
            Console.WriteLine("\nLancement d'une partie : ...\n");

            // Joueur 1
            Console.Write("Joueur 1, veuillez entrer votre nom : ");
            Player1Name = Console.ReadLine();

            Console.WriteLine($"\nBonjour, {Player1Name}");
            Console.WriteLine("Veuillez m'indiquer le fichier contenant vos cartes (CardGameConsole/Decks/) :");
            var deck1 = File.ReadLines("../../Decks/" + Console.ReadLine()).ToList();

            // Joueur 2
            Console.Write("\nJoueur 2, veuillez entrer votre nom : ");
            Player2Name = Console.ReadLine();

            Console.WriteLine($"\nBonjour, {Player2Name}");
            Console.WriteLine("Veuillez m'indiquer le fichier contenant vos cartes (CardGameConsole/Decks/) :");
            var deck2 = File.ReadLines("../../Decks/" + Console.ReadLine()).ToList();

            Game = new Game("../../../CardGameEngine/EffectsScripts/", new ConsoleExternCallbacks(), deck1, deck2);

            Console.WriteLine(Game.Player1.Deck.ToString());
            Console.WriteLine(Game.Player2.Deck.ToString());

            RegisterEventListeners();
            
            Console.WriteLine("Début de partie :");
        }

        private static void RegisterEventListeners()
        {
            //todo event listener pour la pluspart des trucs pour afficher du texte
        }
    }
}