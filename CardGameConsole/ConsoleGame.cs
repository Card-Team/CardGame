using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGameEngine;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents;
using CardGameEngine.EventSystem.Events.GameStateEvents;

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

            var winner = Game.StartGame();

            Console.WriteLine($"{winner} est le grand gagnant de cette compétition intensive, bravo ! ☺");
        }

        private static void RegisterEventListeners()
        {
            Game.EventManager.SubscribeToEvent<StartTurnEvent>(OnTurnStart, postEvent: true);
        }

        private static void OnTurnStart(StartTurnEvent turnEvent)
        {
            Console.WriteLine($"\nTour de {turnEvent.Player.GetName()}");
            turnEvent.Player.PrintHand();

            var finish = false;
            while (!finish)
            {
                Console.WriteLine("\nAppuyez sur 0 pour terminer votre tour");
                Console.WriteLine("Appuyez sur 1 pour lister vos cartes");
                Console.WriteLine("Appuyez sur 2 pour jouer une carte");
                Console.WriteLine("Appuyez sur 3 pour améliorer une carte");

                var choice = InputUtils.ChooseFromList(Enumerable.Range(0, 4).ToList());
                switch (choice)
                {
                    case 0:
                        finish = true;
                        break;
                    case 1:
                        turnEvent.Player.PrintHand();
                        break;
                    case 2:
                        PlayCard(false);
                        break;
                    case 3:
                        PlayCard(true);
                        break;
                }
            }

            Game.EndPlayerTurn();
        }

        private static void PlayCard(bool upgrade)
        {
            var chosen = InputUtils.ChooseFrom(Game.CurrentPlayer, Game.CurrentPlayer.Hand);
            Game.PlayCard(Game.CurrentPlayer, chosen, upgrade);
        }
    }
}