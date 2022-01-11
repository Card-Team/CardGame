using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGameEngine;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems;
using MoonSharp.Interpreter;
using Spectre.Console;

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
            Player1Name = "Simon";
            //
            // Console.WriteLine($"\nBonjour, {Player1Name}");
            // Console.WriteLine("Veuillez m'indiquer le fichier contenant vos cartes (CardGameConsole/Decks/) :");
            var deck1 = File.ReadLines("../../Decks/oui.txt").ToList();
            //
            // // Joueur 2
            // Console.Write("\nJoueur 2, veuillez entrer votre nom : ");
            // Player2Name = Console.ReadLine();
            Player2Name = "Rachelle";
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
                var choice = -1;

                while (choice != 0)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.Write(new Rule($"Tour de [bold]{Game.CurrentPlayer.GetName()}[/]").Centered());
                    PrintInfo(Game.CurrentPlayer, false);
                    var choices = new Dictionary<string, int>
                    {
                        {"Lister votre défausse", 1},
                        {"Lister les information adverses", 2},
                        {"Jouer une carte", 3},
                        {"Améliorer une carte", 4},
                        {"Terminer votre tour", 0},
                    };


                    switch (choice)
                    {
                        case 1:
                            Game.CurrentPlayer.PrintDiscard();
                            break;
                        case 2:
                            PrintInfo(Game.CurrentPlayer.OtherPlayer, true);
                            break;
                        case 3:
                            PlayCard(false);
                            break;
                        case 4:
                            PlayCard(true);
                            break;
                    }

                    choice = InputUtils.ChooseList("Veuillez choisir une action :", choices);
                }

                Game.EndPlayerTurn();
            }
        }

        private static void PrintInfo(Player player, bool sayTurn)
        {
            if (player == Game.CurrentPlayer)
            {
                if (sayTurn)
                    AnsiConsole.Write(new Markup($"\nTour de [bold]{Game.CurrentPlayer.GetName()}[/]").Centered());
                Game.CurrentPlayer.PrintHand();
                AnsiConsole.Write(new Panel(
                        new Markup($"Nombre de cartes dans votre défausse : {Game.CurrentPlayer.Discard.Count}\n" +
                                   $"Nombre de cartes dans votre deck : {Game.CurrentPlayer.Deck.Count}\n" +
                                   $"Nombre de points d'actions: {Game.CurrentPlayer.ActionPoints.Value}"))
                    .Header("Vos informations", Justify.Right));
            }
            else
            {
                AnsiConsole.Write(new Panel(
                        new Markup($"Nombre de cartes dans la main adverse : {player.Hand.Count}\n" +
                                   $"Cartes de la défausse adverse :\n" +
                                   $"Nombre de points d'actions de l'adversaire: {player.ActionPoints.Value}"))
                    .Header("Informations de l'adversaire", Justify.Right));
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
            AnsiConsole.Clear();
        }

        private static void PlayCard(bool upgrade)
        {
            var available = Game.CurrentPlayer.Hand.Where(c => c.Cost.Value <= Game.CurrentPlayer.ActionPoints.Value)
                .ToList();
            if (!upgrade)
            {
                available = available.Where(c => c.CanBePlayed(Game, Game.CurrentPlayer)).ToList();
            }
            else
            {
                available = available.Where(c => c.CurrentLevel.Value < c.MaxLevel).ToList();
            }

            if (available.Count == 0)
            {
                Console.WriteLine("Aucune carte disponible");
                return;
            }

            var chosen = InputUtils.ChooseFrom(Game.CurrentPlayer, available, true);
            if (chosen == null)
            {
                return;
            }

            Game.PlayCard(Game.CurrentPlayer, chosen, upgrade);
        }
    }
}