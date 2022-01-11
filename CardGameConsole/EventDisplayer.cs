using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;
using CardGameEngine.GameSystems;
using Spectre.Console;
using Spectre.Console.Rendering;

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
            WriteEvent(new Markup($"Votre limite de [green]points d'action[/] est maintenant de [bold]{evt.NewMaxPointCount}[/]").RightAligned());
        }

        private static void OnActionPointsEdit(ActionPointsEditEvent evt)
        {
            WriteEvent(new Markup($"Il vous reste [bold]{evt.NewPointCount}[/] [green]points d'action[/]").RightAligned());
        }

        private static void OnCardMarkedUpgrade(CardMarkUpgradeEvent evt)
        {
            WriteEvent(new Markup($"[bold]{evt.Card.Name}[/] est prête à se faire améliorer").RightAligned());
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
                    WriteEvent(new Markup($"La carte [underline]{evt.Card.Name}[/] de [bold]{player.GetName()}[/] se dirige vers [red]la défausse[/]").RightAligned());

                // Deck → Main = Pioche
                else if (evt.SourcePile == player.Deck && evt.DestPile == player.Hand)

                    if (player == ConsoleGame.Game.CurrentPlayer)
                        // Tu pioches
                        WriteEvent(new Markup($"Vous venez de piocher [underline]{evt.Card.Name}[/]").RightAligned());
                    else
                        // L'adversaire pioche
                        WriteEvent(new Markup($"[bold]{player.GetName()}[/] vient de piocher une carte").RightAligned());
            }
        }

        private static void OnCardPlay(CardPlayEvent evt)
        {
            WriteEvent(new Markup($"[underline]{evt.Card.Name}[/] vient d'être activée").RightAligned());
        }

        private static void WriteEvent(IRenderable evt)
        {
            AnsiConsole.Write(evt);
        }

        private static void OnPostCardPlay(CardPlayEvent evt)
        {
            WriteEvent(new Markup($"[underline]L'effet de {evt.Card.Name}[/] a été exécuté avec succès").RightAligned());
        }

        private static void OnCardLevelChange(CardLevelChangeEvent evt)
        {
            // Niveau monte
            if (evt.NewValue > evt.OldValue)
                Console.WriteLine(evt.Card.IsMaxLevel
                    ? $"[underline]{evt.Card.Name}[/] est maintenant au [blue]niveau max ({evt.Card.CurrentLevel})[/]"
                    : $"[underline]{evt.Card.Name}[/] est maintenant au [blue]niveau {evt.Card.CurrentLevel}/{evt.Card.MaxLevel}[/]");

            // Niveau baisse
            else
                WriteEvent(new Markup($"[underline]{evt.Card.Name}[/] est redescendue au [blue]niveau {evt.NewValue}[/]").RightAligned());
        }
    }
}