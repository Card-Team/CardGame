using System;
using System.IO;
using System.Linq;
using CardGameEngine;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems;
using MoonSharp.Interpreter;

namespace CardGameConsole
{
    internal static class ConsoleGame
    {
        public static Game Game;

        public static string Player1Name;
        public static string Player2Name;

        private static Player? _winner;
        private static bool _gameEnded;

        public static void Main(string[] args)
        {
            Console.WriteLine("\nLancement d'une partie : ...\n");

            // Joueur 1
            // Console.Write("Joueur 1, veuillez entrer votre nom : ");
            // Player1Name = Console.ReadLine();
            Player1Name = "oui";
            //
            // Console.WriteLine($"\nBonjour, {Player1Name}");
            // Console.WriteLine("Veuillez m'indiquer le fichier contenant vos cartes (CardGameConsole/Decks/) :");
            var deck1 = File.ReadLines("../../Decks/oui.txt").ToList();
            //
            // // Joueur 2
            // Console.Write("\nJoueur 2, veuillez entrer votre nom : ");
            // Player2Name = Console.ReadLine();
            Player2Name = "non";
            //
            // Console.WriteLine($"\nBonjour, {Player2Name}");
            // Console.WriteLine("Veuillez m'indiquer le fichier contenant vos cartes (CardGameConsole/Decks/) :");
            var deck2 = File.ReadLines("../../Decks/non.txt").ToList();

            Game = new Game("../../../CardGameEngine/EffectsScripts/", new ConsoleExternCallbacks(), deck1, deck2);

            Console.WriteLine(Game.Player1.Deck.ToString());
            Console.WriteLine(Game.Player2.Deck.ToString());

            RegisterEventListeners();
            EventDisplayer.RegisterAllEvents(Game.EventManager);

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
            catch (InvalidOperationException exception)
            {
                if (exception.TargetSite.DeclaringType?.Namespace?.StartsWith("CardGameEngine") == true)
                {
                    ErrorUtils.PrintError(exception);
                }
                else throw;
            }
        }

        private static void GameLoop()
        {
            while (!_gameEnded)
            {
                PrintInfo(Game.CurrentPlayer);
                var turnEnded = false;
                while (!turnEnded)
                {
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("\nAppuyez sur 0 pour terminer votre tour");
                    Console.WriteLine("Appuyez sur 1 pour lister vos cartes + d'autres infos");
                    Console.WriteLine("Appuyez sur 2 pour lister votre défausse");
                    Console.WriteLine("Appuyez sur 3 pour lister les information adverses");
                    Console.WriteLine("Appuyez sur 4 pour jouer une carte");
                    Console.WriteLine("Appuyez sur 5 pour améliorer une carte");

                    var choice = InputUtils.ChooseFromList(Enumerable.Range(0, 6).ToList());
                    Console.WriteLine("---------------------------");
                    switch (choice)
                    {
                        case 0:
                            turnEnded = true;
                            break;
                        case 1:
                            Console.WriteLine("Cartes dans votre main : ");
                            PrintInfo(Game.CurrentPlayer);
                            break;
                        case 2:
                            Console.WriteLine("Votre défausse : ");
                            Game.CurrentPlayer.PrintDiscard();
                            break;
                        case 3:
                            PrintInfo(Game.CurrentPlayer.OtherPlayer);
                            break;
                        case 4:
                            PlayCard(false);
                            break;
                        case 5:
                            PlayCard(true);
                            break;
                    }
                }

                Game.EndPlayerTurn();
            }
        }

        private static void PrintInfo(Player player)
        {
            if (player == Game.CurrentPlayer)
            {
                Console.WriteLine($"\nTour de {Game.CurrentPlayer.GetName()}");
                Game.CurrentPlayer.PrintHand();
                Console.WriteLine($"Nombre de cartes dans votre défausse : {Game.CurrentPlayer.Discard.Count}");
                Console.WriteLine($"Nombre de cartes dans votre deck : {Game.CurrentPlayer.Deck.Count}");
                Console.WriteLine($"Nombre de points d'actions: {Game.CurrentPlayer.ActionPoints.Value}");
            }
            else
            {
                Console.WriteLine($"Nombre de cartes dans la main adverse : {player.Hand.Count}");
                Console.WriteLine($"Nombre de cartes dans le deck adverse : {player.Deck.Count}");

                Console.WriteLine($"Cartes de la défausse adverse :");
                player.PrintDiscard();
                Console.WriteLine($"Nombre de points d'actions de l'adversaire: {player.ActionPoints.Value}");
            }
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
            var available = Game.CurrentPlayer.Hand.Where(c => c.Cost.Value <= Game.CurrentPlayer.ActionPoints.Value)
                .ToList();
            if (!upgrade)
            {
                available = available.Where(c => c.CanBePlayed(Game, Game.CurrentPlayer)).ToList();
            }

            if (available.Count == 0)
            {
                Console.WriteLine("Aucune carte disponible");
                return;
            }

            var chosen = InputUtils.ChooseFrom(Game.CurrentPlayer, available);
            Game.PlayCard(Game.CurrentPlayer, chosen, upgrade);
        }
    }
}