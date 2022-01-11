using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;
using CardGameEngine.GameSystems;
using Spectre.Console;

namespace CardGameConsole
{
    public static class EventDisplayer
    {
        public static void RegisterAllEvents(EventManager eventManager)
        {
            // Carte monte de niveau
            eventManager.SubscribeToEvent<CardLevelChangeEvent>(OnCardLevelChange, postEvent: true);

            // Carte jouée puis effet exécuté
            eventManager.SubscribeToEvent<CardPlayEvent>(OnCardPlay, postEvent: false);
            eventManager.SubscribeToEvent<CardPlayEvent>(OnPostCardPlay, postEvent: true);

            // Déplacement d'une carte
            eventManager.SubscribeToEvent<CardMovePileEvent>(OnCardChangePile, postEvent: true);

            // Mise en amélioration d'une carte 
            eventManager.SubscribeToEvent<CardMarkUpgradeEvent>(OnCardMarkedUpgrade, postEvent: true);

            // Nombre de point d'action
            eventManager.SubscribeToEvent<ActionPointsEditEvent>(OnActionPointsEdit, postEvent: true);

            // Nombre max de points d'actions
            eventManager.SubscribeToEvent<MaxActionPointsEditEvent>(OnMaxActionPointsEdit, postEvent: true);
        }

        private static void OnMaxActionPointsEdit(MaxActionPointsEditEvent evt)
        {
            Console.WriteLine($"Votre limite de points d'action est maintenant de {evt.NewMaxPointCount}");
        }

        private static void OnActionPointsEdit(ActionPointsEditEvent evt)
        {
            Console.WriteLine($"Il vous reste {evt.NewPointCount} points d'action");
        }

        private static void OnCardMarkedUpgrade(CardMarkUpgradeEvent evt)
        {
            Console.WriteLine($"{evt.Card.Name} est prête à se faire améliorer");
        }

        private static void OnCardChangePile(CardMovePileEvent evt)
        {
            List<Player> players = new List<Player>
            {
                ConsoleGame.Game.CurrentPlayer,
                ConsoleGame.Game.CurrentPlayer.OtherPlayer
            };

            foreach (var player in players)
            {
                // Main → Défausse = Défausser/Améliorer
                if (evt.SourcePile == player.Hand && evt.DestPile == player.Discard)
                    Console.WriteLine($"La carte {evt.Card.Name} de {player.GetName()} se dirige vers la défausse");

                // Deck → Main = Pioche
                else if (evt.SourcePile == player.Deck && evt.DestPile == player.Hand)

                    if (player == ConsoleGame.Game.CurrentPlayer)
                        // Tu pioches
                        Console.WriteLine($"Vous venez de piocher {evt.Card.Name}");
                    else
                        // L'adversaire pioche
                        Console.WriteLine($"{player.GetName()} vient de piocher une carte");
            }
        }

        private static void OnCardPlay(CardPlayEvent evt)
        {
            Console.WriteLine($"{evt.Card.Name} vient d'être activée");
        }

        private static void OnPostCardPlay(CardPlayEvent evt)
        {
            Console.WriteLine($"L'effet de {evt.Card.Name} a été exécuté avec succès");
        }

        private static void OnCardLevelChange(CardLevelChangeEvent evt)
        {
            // Niveau monte
            if (evt.NewValue > evt.OldValue)
                Console.WriteLine(evt.Card.IsMaxLevel
                    ? $"{evt.Card.Name} est maintenant au niveau max ({evt.Card.CurrentLevel})"
                    : $"{evt.Card.Name} est maintenant au niveau {evt.Card.CurrentLevel}/{evt.Card.MaxLevel}");

            // Niveau baisse
            else
                Console.WriteLine($"{evt.Card.Name} est redescendue au niveau {evt.NewValue}");
        }
    }
}