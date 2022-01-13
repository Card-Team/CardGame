using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems;
using Spectre.Console;

namespace CardGameConsole
{
    public static class EventDisplayer
    {
        public static List<string> Events { get; } = new List<string>();

        public static void RegisterAllEvents(EventManager eventManager)
        {
            // Carte monte de niveau
            eventManager.SubscribeToEvent<CardLevelChangeEvent>(OnCardLevelChange, postEvent: true);

            // Carte jouée puis effet exécuté
            eventManager.SubscribeToEvent<CardPlayEvent>(OnCardPlay, postEvent: false);
            eventManager.SubscribeToEvent<CardEffectPlayEvent>(OnCardEffectPlay, postEvent: false);
            eventManager.SubscribeToEvent<CardEffectPlayEvent>(OnPostCardEffectPlay, postEvent: true);
            eventManager.SubscribeToEvent<CardPlayEvent>(OnPostCardPlay, postEvent: true);

            // Déplacement d'une carte
            eventManager.SubscribeToEvent<CardMovePileEvent>(OnCardChangePile, postEvent: true);

            // Mise en amélioration d'une carte 
            eventManager.SubscribeToEvent<CardMarkUpgradeEvent>(OnCardMarkedUpgrade, postEvent: true);

            eventManager.SubscribeToEvent<CardUnMarkUpgradeEvent>(OnCardRemovedMarkedUpgrade, postEvent: true);

            // Bouclage du deck
            eventManager.SubscribeToEvent<DeckLoopEvent>(OnDeckLoop, postEvent: true);

            // Nombre de point d'action
            eventManager.SubscribeToEvent<ActionPointsEditEvent>(OnActionPointsEdit, postEvent: true);

            // Nombre max de points d'actions
            eventManager.SubscribeToEvent<MaxActionPointsEditEvent>(OnMaxActionPointsEdit, postEvent: true);
        }

        private static void OnMaxActionPointsEdit(MaxActionPointsEditEvent evt)
        {
            WriteEvent($"Votre limite de [green]points d'action[/] est maintenant de [bold]{evt.NewMaxPointCount}[/]");
        }

        private static void OnActionPointsEdit(ActionPointsEditEvent evt)
        {
            WriteEvent(
                $"Vous avez désormais [bold]{evt.NewPointCount}[/] [green]points d'action (sur {evt.Player.MaxActionPoints.Value})[/]");
        }

        private static void OnCardMarkedUpgrade(CardMarkUpgradeEvent evt)
        {
            WriteEvent($"[underline]{evt.Card.Name}[/] est prête à se faire améliorer");
        }

        private static void OnCardRemovedMarkedUpgrade(CardUnMarkUpgradeEvent evt)
        {
            WriteEvent($"[underline]{evt.Card.Name}[/] [bold]ne sera pas améliorée[/]");
        }

        private static void OnDeckLoop(DeckLoopEvent evt)
        {
            WriteEvent(
                $"Le deck de [bold]{evt.Player.GetName()}[/] a bouclé et contient maintenant [bold]{evt.Player.Deck.Count}[/] cartes");
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
                    WriteEvent(
                        $"La carte [underline]{evt.Card.Name}[/] de [bold]{player.GetName()}[/] se dirige vers [red]la défausse[/]");

                // Deck → Main = Pioche
                else if (evt.SourcePile == player.Deck && evt.DestPile == player.Hand)

                    if (player == ConsoleGame.Game.CurrentPlayer)
                        // Tu pioches
                        WriteEvent($"Vous venez de piocher [underline]{evt.Card.Name}[/]");
                    else
                        // L'adversaire pioche
                        WriteEvent($"[bold]{player.GetName()}[/] vient de piocher une carte");
            }
        }

        private static void OnCardPlay(CardPlayEvent evt)
        {
            WriteEvent($"[underline]{evt.Card.Name}[/] vient d'être jouée par [bold]{evt.WhoPlayed.GetName()}[/]");
        }

        private static void OnCardEffectPlay(CardEffectPlayEvent evt)
        {
            WriteEvent(
                $"L'effet de [underline]{evt.Card.Name}[/] vient d'être activé par [bold]{evt.WhoPlayed.GetName()}[/]");
        }

        private static void OnPostCardEffectPlay(CardEffectPlayEvent evt)
        {
            WriteEvent($"L'effet de [underline]{evt.Card.Name}[/] a terminé son execution");
        }

        private static void WriteEvent(string evt)
        {
            Events.Add(evt);
        }

        public static Panel? DumpEvents()
        {
            return Events.Count == 0 ? null : new Panel(new Markup(string.Join("\n", Events)).Alignment(Justify.Right));
        }

        public static void ClearEvents()
        {
            Events.Clear();
        }

        private static void OnPostCardPlay(CardPlayEvent evt)
        {
            WriteEvent($"La carte [underline]{evt.Card.Name}[/] a fini d'etre jouée");
        }

        private static void OnCardLevelChange(CardLevelChangeEvent evt)
        {
            // Niveau monte
            if (evt.NewValue > evt.OldValue)
                WriteEvent(evt.Card.IsMaxLevel
                    ? $"[underline]{evt.Card.Name}[/] est maintenant au [blue]niveau max ({evt.Card.CurrentLevel})[/]"
                    : $"[underline]{evt.Card.Name}[/] est maintenant au [blue]niveau {evt.Card.CurrentLevel}/{evt.Card.MaxLevel}[/]");

            // Niveau baisse
            else
                WriteEvent($"[underline]{evt.Card.Name}[/] est redescendue au [blue]niveau {evt.NewValue}[/]");
        }
    }
}