using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGameEngine;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems;
using MoonSharp.Interpreter;

namespace CardGameConsole
{
    internal class ConsoleGame
    {
        public static Game Game;

        private Player? _winner;

        public static string Player1Name;
        public static string Player2Name;
        private static bool _gameEnded;

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

            try
            {
                Game.StartGame();

                GameLoop();
            }
            catch (ScriptRuntimeException exception)
            {
                ErrorUtils.PrintError(exception);
            }
        }

        private static void GameLoop()
        {
            while (!_gameEnded)
            {
                PrintInfo();
                var turnEnded = false;
                while (!turnEnded)
                {
                    Console.WriteLine("\nAppuyez sur 0 pour terminer votre tour");
                    Console.WriteLine("Appuyez sur 1 pour lister vos cartes + d'autres infos");
                    Console.WriteLine("Appuyez sur 2 pour jouer une carte");
                    Console.WriteLine("Appuyez sur 3 pour améliorer une carte");

                    var choice = InputUtils.ChooseFromList(Enumerable.Range(0, 4).ToList());
                    switch (choice)
                    {
                        case 0:
                            turnEnded = true;
                            break;
                        case 1:
                            PrintInfo();
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
        }

        private static void PrintInfo()
        {
            Console.WriteLine($"\nTour de {Game.CurrentPlayer.GetName()}");
            Game.CurrentPlayer.PrintHand();
            Console.WriteLine($"Nombre de cartes dans votre défausse : {Game.CurrentPlayer.Discard.Count}");
            Console.WriteLine($"Nombre de cartes dans votre deck : {Game.CurrentPlayer.Deck.Count}");
            Console.WriteLine($"Nombre de points d'actions: {Game.CurrentPlayer.ActionPoints.Value}");
        }

        private static void RegisterEventListeners()
        {
            Game.EventManager.SubscribeToEvent<StartTurnEvent>(OnTurnStart, postEvent: true);
            Game.EventManager.SubscribeToEvent<PlayerWinEvent>(OnGameEnd, postEvent: true);
        }

        private static void OnGameEnd(PlayerWinEvent evt)
        {
            Console.WriteLine($"{evt.Player} est le grand gagnant de cette compétition intensive, bravo ! ☺");
            _gameEnded = true;
        }

        private static void OnTurnStart(StartTurnEvent turnEvent)
        {
        }

        private static void PlayCard(bool upgrade)
        {
            var chosen = InputUtils.ChooseFrom(Game.CurrentPlayer,
                Game.CurrentPlayer.Hand.Where(c => c.CanBePlayed(Game, Game.CurrentPlayer)));
            Game.PlayCard(Game.CurrentPlayer, chosen, upgrade);
        }
    }
}