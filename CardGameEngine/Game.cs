using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine.Cards;
using CardGameEngine.Cards.CardPiles;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.CardEvents;
using CardGameEngine.EventSystem.Events.GameStateEvents;
using CardGameEngine.GameSystems;
using CardGameEngine.GameSystems.Effects;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine
{
    /// <summary>
    /// Classe principale du projet représentant une partie
    /// </summary>
    public class Game
    {
        private const string VictoryCardEffectId = "_victory";

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
        public EventManager EventManager { get; }

        /// <summary>
        /// Base de données de tous les effets
        /// </summary>
        /// <seealso cref="Effect"/>
        internal EffectsDatabase EffectsDatabase { get; }

        /// <summary>
        /// Callbacks externes au moteurs, ce champ va être donné par l'application externe.
        /// </summary>
        private readonly IExternCallbacks _externCallbacks;


        public Game(string effectFolder, IExternCallbacks externCallbacks, IEnumerable<string> deck1,
            IEnumerable<string> deck2)
        {
            EventManager = new EventManager();

            _externCallbacks = externCallbacks;

            EffectsDatabase = new EffectsDatabase(effectFolder);
            var cards1 = deck1.Select(s => EffectsDatabase[s]()).Select(e => new Card(this, e)).ToList();
            var cards2 = deck2.Select(s => EffectsDatabase[s]()).Select(e => new Card(this, e)).ToList();


            //TODO Add carte victoire

            Player1 = new Player(this, cards1);
            Player2 = new Player(this, cards2);

            CurrentPlayer = Player1;
        }

        public void StartGame()
        {
            for (var i = 0; i < 2; i++)
            {
                Player1.DrawCard();
                Player2.DrawCard();
            }

            foreach (var card in Player1.Cards.Concat(Player2.Cards))
            {
                card.OnCardCreate();
            }

            StartPlayerTurn(CurrentPlayer);
        }

        /// <summary>
        /// Fait gagner la partie a un joueur
        /// </summary>
        /// <param name="playerToWin">La joueur à faire gagner</param>
        internal void MakeWin(Player playerToWin)
        {
            CurrentPlayer = null;

            var evt = new PlayerWinEvent(playerToWin);
            using var post = EventManager.SendEvent(evt);
        }

        /// <summary>
        /// Lance le tour d'un joueur
        /// </summary>
        /// <param name="player">Le joueur qui doit jouer</param>
        private void StartPlayerTurn(Player player)
        {
            var turnEvent = new StartTurnEvent(player);
            using (var postSender = EventManager.SendEvent(turnEvent))
            {
                CurrentPlayer = postSender.Event.Player;
                CurrentPlayer.ActionPoints.TryChangeValue(CurrentPlayer.MaxActionPoints.Value);
                if (CurrentPlayer.Deck.IsEmpty)
                {
                    CurrentPlayer.LoopDeck();
                }

                CurrentPlayer.DrawCard();
            }
        }

        /// <summary>
        /// Essaye de finir le tour du joueur actuel
        /// </summary>
        internal bool TryEndPlayerTurn()
        {
            var turnEvent = new EndTurnEvent(CurrentPlayer);
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
        [MoonSharpVisible(false)]
        public bool PlayCard(Player player, Card card, bool upgrade)
        {
            if (card.IsVirtual)
            {
                throw new InvalidOperationException(
                    $"La carte {card} a eu une tentative d'activation alors qu'elle est virtuelle");
            }
            if (CurrentPlayer != player)
            {
                throw new InvalidOperationException(
                    $"Player {player} tried to play a card when it's turn of {CurrentPlayer}");
            }

            if (!player.Hand.Contains(card))
            {
                throw new InvalidOperationException(
                    $"Player {player} tried to play card {card} but it's not in their hand");
            }

            if (upgrade && card.IsMaxLevel)
            {
                throw new InvalidOperationException(
                    $"Tentative d'amélioration de la carte {card} alors qu'elle est au niveau maximum ({card.CurrentLevel})");
            }
            

            if (!upgrade && card.CanBePlayed(this, player) == false) return false;

            var newVal = Math.Max(0, player.ActionPoints.Value - card.Cost.Value);
            player.ActionPoints.TryChangeValue(newVal);

            return upgrade ? UpgradeCard(card) : PlayCardEffect(player, card, player.Hand, player.Discard);
        }

        [MoonSharpVisible(true)]
        internal bool PlayCardVirtual(Player effectowner, Card card)
        {
            if (!card.IsVirtual)
            {
                throw new InvalidOperationException(
                    $"PlayCardVirtual appelé avec une carte non virtuelle ({card})");
            }

            if (card.CanBePlayed(this, effectowner) == false) ;
            
            return PlayCardEffect(effectowner, card, null,null);
        }
        
        [MoonSharpVisible(true)]
        internal bool PlayCardFromEffect(Player effectowner, Card card)
        {
            if (card.IsVirtual)
            {
                throw new InvalidOperationException(
                    $"PlayCardFromEffect appelé avec une carte virtuelle ({card})");
            }

            if (card.CanBePlayed(this, effectowner) == false) ;

            return PlayCardEffect(effectowner, card, GetPileOf(card), effectowner.Discard);
        }
        
        

        private bool PlayCardEffect(Player originalPlayer, Card card, CardPile? discardSource, DiscardPile? discardGoal)
        {
            var playResult = true;
            var playEvent = new CardPlayEvent(originalPlayer, card);

            using (var post = EventManager.SendEvent(playEvent))
            {
                if (post.Event.Cancelled)
                {
                    return false;
                }

                var doitDefausser = post.Event.Card.DoEffect(this, post.Event.WhoPlayed);

                //défaussement

                if (doitDefausser && discardSource != null && discardSource.Contains(post.Event.Card))
                {
                    if (discardGoal == null)
                    {
                        throw new InvalidOperationException(
                            $"La carte {post.Event.Card} doit etre défaussé de {discardSource} mais aucune déstination n'a été donnée");
                    }

                    if (!discardSource.MoveTo(discardGoal, post.Event.Card, 0))
                    {
                        playResult = false;
                    }
                }
                else
                {
                    playResult = false;
                }
            }

            return playResult;
        }

        private bool UpgradeCard(Card card)
        {
            var evt = new CardMarkUpgradeEvent(card);
            using (var post = EventManager.SendEvent(evt))
            {
                if (post.Event.Cancelled)
                {
                    return false;
                }

                var toUp = post.Event.Card;

                var location = GetPileOf(toUp);
                var owner = GetCurrentOwner(toUp);

                return owner.Discard.MoveForUpgrade(location, toUp);
            }
        }

        internal Player GetCurrentOwner(Card card)
        {
            if (Player1.Cards.Contains(card))
            {
                return Player1;
            }

            if (Player2.Cards.Contains(card))
            {
                return Player2;
            }

            throw new InvalidOperationException($"La carte {card} n'appartient a aucun joueur");
        }

        internal CardPile GetPileOf(Card card)
        {
            var currentOwner = GetCurrentOwner(card);
            if (currentOwner.Hand.Contains(card)) return currentOwner.Hand;
            if (currentOwner.Deck.Contains(card)) return currentOwner.Deck;
            if (currentOwner.Discard.Contains(card)) return currentOwner.Discard;

            throw new InvalidOperationException($"La carte {card} n'appartient a aucun joueur");
        }


        /// <summary>
        /// Révèle une la carte card au joueur originalPlayer <b>une seule fois</b>
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        [MoonSharpVisible(true)]
        internal void RevealCard(Player player, Card card)
        {
            _externCallbacks.ExternShowCard(player,card);
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
        /// Demande au joueur originalPlayer de choisir une carte parmi la liste cards et renvoie son choix
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
            i = i - 1; //parce que lua
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
                    var cards = GetValidTargets(target);
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

        public string? GetScriptByName(string effectName)
        {
            return EffectsDatabase.GetScript(effectName);
        }

        private List<Card> GetValidTargets(Target target)
        {
            if (target.IsAutomatic)
            {
                throw new InvalidOperationException("TargetsExists ne doit etre appelé que pour des cibles manuelles");
            }

            var allCards = Player1.Cards.Concat(Player2.Cards);

            return allCards.Where(target.IsValidTarget).ToList();
        }

        internal bool LuaTargetsExists(Effect effect, Player effectOwner, IEnumerable<int> list)
        {
            return list.All(targetId => GetValidTargets(effect.AllTargets[targetId - 1]).Any());
        }
    }
}