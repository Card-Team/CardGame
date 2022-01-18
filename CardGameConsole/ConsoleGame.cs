using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGameEngine;
using CardGameEngine.Cards;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems;
using CardGameEngine.GameSystems.Effects;
using MoonSharp.Interpreter;
using Spectre.Console;

namespace CardGameConsole
{
    internal static class ConsoleGame
    {
        public static Game Game;

        public static string Player1Name;
        public static string Player2Name;

        public static Card Player1Vic;
        public static Card Player2Vic;

        public static Player? Winner;

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

            try
            {
                Game = new Game("../../../CardGameEngine/EffectsScripts/", new ConsoleExternCallbacks(), deck1, deck2);

                Console.WriteLine(Game.Player1.Deck.ToString());
                Console.WriteLine(Game.Player2.Deck.ToString());
                RegisterEventListeners();
                EventDisplayer.RegisterAllEvents(Game.EventManager);

                Console.WriteLine("Début de partie :");


                Game.StartGame();
                EventDisplayer.ClearEvents();
                Player1Vic = Game.Player1.Cards.First(f => f.EffectId == Game.VictoryCardEffectId);
                Player2Vic = Game.Player2.Cards.First(f => f.EffectId == Game.VictoryCardEffectId);
                GameLoop();
                AnsiConsole.Write(new Rule($"{Winner?.GetName()} a gagné la partie !").Centered());
            }
            catch (InvalidEffectException exc)
            {
                ErrorUtils.PrintError(exc);
            }
            catch (InterpreterException exception)
            {
                ErrorUtils.PrintError(exception);
            }
            catch (LuaException exception)
            {
                ErrorUtils.PrintError(exception);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.TargetSite.DeclaringType?.Namespace?.StartsWith("CardGameEngine") == true)
                {
                    ErrorUtils.PrintError(exception);
                }
                else
                {
                    AnsiConsole.WriteException(exception);
                    ErrorUtils.DumpEvents();
                }
            }
            catch (Exception exception)
            {
                AnsiConsole.WriteException(exception);
                ErrorUtils.DumpEvents();
            }
        }

        private static void GameLoop()
        {
            while (Winner == null)
            {
                if (DoTurnPrompt(Game.CurrentPlayer)) return;
                EventDisplayer.ClearEvents();
                Game.EndPlayerTurn();
            }
        }

        public static bool DoTurnPrompt(Player player, bool isChaining = false)
        {
            var choice = -1;

            while (choice != 0)
            {
                switch (choice)
                {
                    case 3:
                        PlayCard(player, false);
                        if (isChaining) return true;
                        break;
                    case 4:
                        PlayCard(player, true);
                        break;
                }

                AnsiConsole.Clear();
                var title = isChaining ? $"Chainage de {player.GetName()}" : $"Tour de [bold]{player.GetName()}[/]";
                AnsiConsole.Write(new Rule(title).Centered());

                player.PrintHand();
                AnsiConsole.Write(
                    new Columns(GetInfos(player),
                        GetVictoryInfo(player),
                        (EventDisplayer.DumpEvents() ?? new Panel("")).Expand().Border(BoxBorder.None)));

                if (Winner != null) return true;

                var choices = new Dictionary<string, int>
                {
                    { "Lister votre défausse", 1 },
                    { "Lister les information adverses", 2 }
                };

                if (player.Hand.CostPlayable().Playable().Any()) choices["Jouer une carte"] = 3;

                if (!isChaining && player.Hand.CostPlayable().Upgradable().Any()) choices["Améliorer une carte"] = 4;

                choices["Terminer votre tour"] = 0;

                switch (choice)
                {
                    case 1:
                        player.PrintDiscard();
                        break;
                    case 2:
                        AnsiConsole.Write(GetInfos(player.OtherPlayer));
                        break;
                }

                choice = InputUtils.ChooseList("Veuillez choisir une action :", choices);
            }

            return false;
        }

        private static Panel GetVictoryInfo(Player player)
        {
            var progress = new BarChart()
                .Width(Player1Vic.MaxLevel + 60)
                .WithMaxValue(Player1Vic.MaxLevel);

            if (player == Game.Player1)
            {
                progress.AddItem("Votre progression", Player1Vic.CurrentLevel.Value, Color.Green)
                    .AddItem($"La progression de {Game.Player2.GetName()}", Player2Vic.CurrentLevel.Value, Color.Red);
            }
            else
            {
                progress.AddItem("Votre progression", Player2Vic.CurrentLevel.Value, Color.Green)
                    .AddItem($"La progression de {Game.Player1.GetName()}", Player1Vic.CurrentLevel.Value, Color.Red);
            }


            return new Panel(progress).Header($"Progression vers la victoire (max:{Player1Vic.MaxLevel})")
                .HeaderAlignment(Justify.Center);
        }

        private static Panel GetInfos(Player player)
        {
            if (player == Game.CurrentPlayer)
            {
                return new Panel(new Markup($"Nombre de cartes dans votre défausse : {player.Discard.Count}\n" +
                                            $"Nombre de cartes dans votre deck : {player.Deck.Count}\n" +
                                            $"Nombre de [green]points d'action[/] : {player.ActionPoints.Value}/{player.MaxActionPoints.Value}"))
                    .Header("Vos informations", Justify.Right);
            }
            else
            {
                return new Panel(new Markup($"Nombre de cartes dans la main adverse : {player.Hand.Count}\n" +
                                            $"Cartes de la défausse adverse : {player.Discard.Count}\n" +
                                            $"Nombre de [green]points d'action[/] de l'adversaire : {player.ActionPoints.Value}/{player.MaxActionPoints.Value}"))
                    .Header("[red]Informations de l'adversaire[/]", Justify.Right);
            }
        }

        private static void RegisterEventListeners()
        {
            Game.EventManager.SubscribeToEvent<StartTurnEvent>(OnTurnStart, postEvent: true);
        }

        private static void OnTurnStart(StartTurnEvent turnEvent)
        {
        }

        private static void PlayCard(Player player, bool upgrade)
        {
            var cards = player.Hand.Where(c => Game.CanPlay(player, c, upgrade)).ToList();

            if (cards.Count == 0)
            {
                Console.WriteLine("Aucune carte disponible");
                return;
            }

            var chosen = InputUtils.ChooseFrom(player, cards, true);
            if (chosen == null)
            {
                return;
            }

            Game.PlayCard(player, chosen, upgrade);
        }
    }
}