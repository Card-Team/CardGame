using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.CardEvents;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems;
using CardGameEngine.GameSystems.Effects;
using CardGameEngine.GameSystems.Targeting;

namespace CardGameEngine
{
    /// <summary>
    /// Classe principale du projet représentant une partie
    /// </summary>
    public class Game
    {
        public const int DefaultMaxActionPoint = 5;


        /// <summary>
        /// Le joueur en train de jouer
        /// </summary>
        public Player CurrentPlayer { get; internal set; }

        /// <summary>
        /// Le joueur 1
        /// </summary>
        public Player Player1 { get; }

        /// <summary>
        /// Le joueur 2
        /// </summary>
        public Player Player2 { get; }

        /// <summary>
        /// Gestionnaire des évènements de la partie
        /// </summary>
        internal EventManager EventManager { get; }

        /// <summary>
        /// Base de données de tous les effets
        /// </summary>
        /// <seealso cref="Effect"/>
        internal EffectsDatabase EffectsDatabase { get; }

        /// <summary>
        /// Callbacks externes au moteurs, ce champ va être donné par l'application externe.
        /// </summary>
        private readonly IExternCallbacks _externCallbacks;


        public Game(string effectFolder, string name1, List<string> deck1, string name2, List<string> deck2)
        {
            EventManager = new EventManager();

            EffectsDatabase = new EffectsDatabase(effectFolder);
            List<Card> cards1 = deck1.Select(s => EffectsDatabase[s]()).Select(e => new Card(e, EventManager)).ToList();
            List<Card> cards2 = deck2.Select(s => EffectsDatabase[s]()).Select(e => new Card(e, EventManager)).ToList();

            //TODO Add carte victoire

            Player1 = new Player(this, name1, cards1);
            Player2 = new Player(this, name2, cards2);
        }

        /// <summary>
        /// Teste si un joueur a gagné la partie
        /// </summary>
        /// <param name="playerToCheck">La joueur à tester</param>
        /// <returns>True si le joueur a gagné</returns>
        private bool CheckHasWon(Player playerToCheck)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lance le tour d'un joueur
        /// </summary>
        /// <param name="player">Le joueur qui doit jouer</param>
        private void StartPlayerTurn(Player player)
        {
            StartTurnEvent turnEvent = new StartTurnEvent(player);
            using (var postSender = EventManager.SendEvent(turnEvent))
            {
                CurrentPlayer = postSender.Event.Player;
                CurrentPlayer.ActionPoints.TryChangeValue(CurrentPlayer.MaxActionPoints.Value);
                CurrentPlayer.DrawCard();
            }
        }

        /// <summary>
        /// Essaye de finir le tour du joueur actuel
        /// </summary>
        internal bool TryEndPlayerTurn()
        {
            EndTurnEvent turnEvent = new EndTurnEvent(CurrentPlayer);
            using (var postEvent = EventManager.SendEvent(turnEvent))
            {
                if (postEvent.Event.Cancelled)
                    return false;
            }

            StartPlayerTurn(CurrentPlayer.OtherPlayer);
            return true;
        }

        /// <summary>
        /// Termine le tour du joueur
        /// </summary>
        public void EndPlayerTurn()
        {
            EndTurnEvent turnEvent = new EndTurnEvent(CurrentPlayer);
            using (EventManager.SendEvent(turnEvent))
            {
            }

            StartPlayerTurn(CurrentPlayer.OtherPlayer);
        }

        /// <summary>
        /// Un joueur utilise une carte
        /// </summary>
        /// <param name="player">Le joueur</param>
        /// <param name="card">La carte jouée</param>
        /// <param name="upgrade">Améliore ou joue</param>
        public void PlayCard(Player player, Card card, bool upgrade)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Révèle une la carte card au joueur player <b>une seule fois</b>
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        private void RevealCard(Player player, Card card)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Un joueur active un artefact
        /// </summary>
        /// <param name="player">Le joueur</param>
        /// <param name="artefact">L'artefact activé</param>
        public void ActivateArtifact(Player player, Artefact artefact)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Demande au joueur player de choisir une carte parmi la liste cards et renvoie son choix
        /// </summary>
        /// <param name="player">Le joueur a qui demander</param>
        /// <param name="cards">La liste de cartes parmi lesquelles choisir</param>
        /// <returns></returns>
        public Card ChooseBetween(Player player, List<Card> cards)
        {
            //todo cartes virtuelles
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fonction lua qui résout une cible
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        internal ITargetable LuaAskForTarget(Effect effect, Player effectOwner, int i)
        {
            var target = effect.AllTargets[i];
            ITargetable resolved;

            if (target.IsAutomatic)
            {
                resolved = target.GetAutomaticTarget();
            }
            else
            {
                if (target.TargetType == TargetTypes.Card)
                {
                    var allCards = Player1.Hand
                        .Concat(Player1.Deck)
                        .Concat(Player1.Discard)
                        .Concat(Player2.Hand)
                        .Concat(Player2.Deck)
                        .Concat(Player2.Discard);

                    var cards = allCards.Where(c => target.IsValidTarget(c)).ToList();
                    resolved = _externCallbacks.ExternCardAskForTarget(effectOwner, target.Name, cards);
                }
                else
                {
                    resolved = _externCallbacks.ExternPlayerAskForTarget(effectOwner, target.Name);
                }
            }

            var targetingEvent = new TargetingEvent(target, resolved);
            using var postSender = EventManager.SendEvent(targetingEvent);
            resolved = postSender.Event.ResolvedTarget;
            return resolved;
        }
    }
}