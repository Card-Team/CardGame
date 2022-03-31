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
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine
{
    /// <summary>
    /// Classe principale du projet représentant une partie
    /// </summary>
    public class Game
    {
        public const string VictoryCardEffectId = "_victoire";

        public const int DefaultMaxActionPoint = 5;

        private int _maxId = 0;

        private readonly List<Card> _allCards;

        /// <summary>
        /// Le joueur en train de jouer (c'est son tour)
        /// </summary>
        public Player CurrentPlayer { get; internal set; }

        /// <summary>
        ///     Le joueur qui a le droit de jouer
        /// </summary>
        public Player AllowedToPlayPlayer { get; internal set; }

        /// <summary>
        ///     Est ce que l'on est en train de chainer
        /// </summary>
        public bool IsInChain => ChainCounter > 0;

        /// <summary>
        ///     La pile des cartes actuellement chainées
        /// </summary>
        public Stack<Card> ChainStack { get; }

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
        [MoonSharpVisible(false)]
        public EventManager EventManager { get; }

        /// <summary>
        /// Base de données de tous les effets
        /// </summary>
        /// <seealso cref="Effect"/>
        internal EffectsDatabase EffectsDatabase { get; }

        /// <summary>
        ///     Toutes les cartes de la partie
        /// </summary>
        public IReadOnlyList<Card> AllCards => _allCards;

        /// <summary>
        /// Callbacks externes au moteurs, ce champ va être donné par l'application externe.
        /// </summary>
        private readonly IExternCallbacks _externCallbacks;

        public int ChainCounter { get; private set; }


        [MoonSharpVisible(false)]
        public Game(string effectFolder, IExternCallbacks externCallbacks, IEnumerable<string> deck1,
            IEnumerable<string> deck2)
        {
            EventManager = new EventManager();

            _externCallbacks = externCallbacks;
            _allCards = new List<Card>();
            ChainStack = new Stack<Card>();

            EffectsDatabase = new EffectsDatabase(effectFolder, _externCallbacks.DebugPrint);
            var cards1 = deck1.Where(s => !s.StartsWith("_"))
                .Prepend(VictoryCardEffectId)
                .Select(s => EffectsDatabase[s]())
                .Select(e =>
                {
                    var card = new Card(this, e, _maxId++);
                    _allCards.Add(card);
                    return card;
                }).ToList();
            var cards2 = deck2.Where(s => !s.StartsWith("_")).Prepend(VictoryCardEffectId)
                .Select(s => EffectsDatabase[s]())
                .Select(e =>
                {
                    var card = new Card(this, e, _maxId++);
                    _allCards.Add(card);
                    return card;
                }).ToList();

            Player1 = new Player(this, cards1, 0);
            Player2 = new Player(this, cards2, 1);

            CurrentPlayer = Player1;
        }

        [MoonSharpVisible(false)]
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
        /// Fait gagner la partie a un joueur apres la fin de ce tour
        /// </summary>
        /// <param name="playerToWin">La joueur à faire gagner</param>
        [MoonSharpVisible(true)]
        internal void MakeWin(Player playerToWin)
        {
            EventManager.Disabled = true;
            _externCallbacks.ExternGameEnded(playerToWin);
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

                AllowedToPlayPlayer = postSender.Event.Player;
            }
        }

        /// <summary>
        /// Essaye de finir le tour du joueur actuel
        /// </summary>
        [MoonSharpVisible(true)]
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
        /// Externe : Termine le tour du joueur
        /// </summary>
        [MoonSharpVisible(false)]
        public void EndPlayerTurn()
        {
            EndTurnEvent turnEvent = new EndTurnEvent(CurrentPlayer);
            using (EventManager.SendEvent(turnEvent))
            {
            }

            StartPlayerTurn(CurrentPlayer.OtherPlayer);
        }
        //todo tableau de proposition de chainage autorisé pour une carte

        /// <summary>
        /// Externe : Un joueur utilise une carte
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

            if (AllowedToPlayPlayer != player)
            {
                throw new InvalidOperationException(
                    $"Player {player} tried to play a card when it's turn of {CurrentPlayer}");
            }

            if (!player.Hand.Contains(card))
            {
                throw new InvalidOperationException(
                    $"Player {player} tried to play card {card} but it's not in their hand");
            }

            if (player.ActionPoints.Value < card.Cost.Value)
                throw new InvalidOperationException(
                    $"Le joueur n'a pas assez de Points d'actions pour jouer la carte (il en faut {card.Cost.Value} et il en a {player.ActionPoints.Value})");

            if (upgrade && card.IsMaxLevel)
            {
                throw new InvalidOperationException(
                    $"Tentative d'amélioration de la carte {card} alors qu'elle est au niveau maximum ({card.CurrentLevel})");
            }

            if (upgrade && IsInChain)
                throw new InvalidOperationException("Tentative d'amélioration pendant une chaine");

            if ((card.ChainMode.Value == ChainMode.StartChain
                 || card.ChainMode.Value == ChainMode.NoChain) && IsInChain)
                throw new InvalidOperationException("Tentative de chainage d'une carte non chainable");

            if (!upgrade && card.CanBePlayed(player) == false)
                throw new InvalidOperationException(
                    "La carte n'est pas jouable (précondition fausse)");

            if (ChainStack.Contains(card))
                throw new InvalidOperationException("Tentative de chainage d'une carte déja en train d'être chainée");

            var result = upgrade ? UpgradeCard(player, card) : PlayCard(player, card);
            if (AllowedToPlayPlayer != CurrentPlayer)
                //fin de chaine apres action
                AllowedToPlayPlayer = CurrentPlayer;
            return result;
        }


        [MoonSharpVisible(true)]
        internal bool PlayCard(Player effectowner, Card card, CardPile? discardSource = null,
            DiscardPile? discardGoal = null)
        {
            discardSource ??= effectowner.Hand;
            discardGoal ??= effectowner.Discard;

            if (CanPlay(effectowner, card, false) == false) return false;

            var playEvt = new CardPlayEvent(effectowner, card);
            using (var post = EventManager.SendEvent(playEvt))
            {
                if (post.Event.Cancelled)
                    return false;
                var player = post.Event.WhoPlayed;
                card = post.Event.Card;

                // points d'actions
                var newVal = Math.Max(0, player.ActionPoints.Value - card.Cost.Value);
                player.ActionPoints.TryChangeValue(newVal);

                //effet
                var shouldDiscard = PlayCardEffect(effectowner, card);

                //défausse
                if (shouldDiscard) discardSource.MoveTo(discardGoal, card, 0);

                return true;
            }
        }


        /// <summary>
        ///     Active l'effet de la carte
        /// </summary>
        /// <param name="effectowner"></param>
        /// <param name="card"></param>
        /// <param name="discardSource"></param>
        /// <param name="discardGoal"></param>
        /// <returns></returns>
        [MoonSharpVisible(true)]
        internal bool PlayCardEffect(Player effectowner, Card card)
        {
            var playEvent = new CardEffectPlayEvent(effectowner, card);
            using (var post = EventManager.SendEvent(playEvent))
            {
                if (card.ChainMode.Value == ChainMode.MiddleChain && IsInChain
                    || card.ChainMode.Value == ChainMode.StartChain
                    || card.ChainMode.Value == ChainMode.StartOrMiddleChain)
                    ChainOpportunity(effectowner.OtherPlayer, card);
                if (post.Event.Cancelled || card.CanBePlayed(effectowner) == false)
                {
                    return false;
                }

                return post.Event.Card.DoEffect(post.Event.WhoPlayed);
            }
        }


        internal void ChainOpportunity(Player player, Card card)
        {
            using (var postchain = EventManager.SendEvent(new ChainingEvent(player)))
            {
                ChainCounter++;
                ChainStack.Push(card);
                AllowedToPlayPlayer = AllowedToPlayPlayer.OtherPlayer;
                if (!_externCallbacks.ExternChainOpportunity(postchain.Event.Chainer))
                {
                    //si abandon de chaine , on rechange
                }

                AllowedToPlayPlayer = AllowedToPlayPlayer.OtherPlayer;
                ChainStack.Pop();
                ChainCounter--;
            }
        }

        private bool UpgradeCard(Player upgrader, Card card)
        {
            if (CanPlay(upgrader, card, true) == false) return false;

            var evt = new CardMarkUpgradeEvent(card);
            using (var post = EventManager.SendEvent(evt))
            {
                if (post.Event.Cancelled)
                {
                    return false;
                }

                var toUp = post.Event.Card;
                var owner = GetCurrentOwner(toUp);

                var newVal = Math.Max(0, owner.ActionPoints.Value - card.Cost.Value);
                owner.ActionPoints.TryChangeValue(newVal);

                var location = GetPileOf(toUp);

                return owner.Discard.MoveForUpgrade(location, toUp);
            }
        }

        [MoonSharpVisible(true)]
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

        [MoonSharpVisible(true)]
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
            _externCallbacks.ExternShowCard(player, card);
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
        ///     Renvoi vrai si le joueur peut jouer la carte
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <param name="upgrade"></param>
        /// <returns></returns>
        public bool CanPlay(Player player, Card card, bool upgrade)
        {
            if (player.ActionPoints.Value < card.Cost.Value) return false;

            return upgrade switch
            {
                true when player != CurrentPlayer => false,
                true => !card.IsMaxLevel,
                false => card.CanBePlayed(player)
            };
        }

        /// <summary>
        /// Demande au joueur originalPlayer de choisir une carte parmi la liste cards et renvoie son choix
        /// </summary>
        /// <param name="player">Le joueur a qui demander</param>
        /// <param name="cards">La liste de cartes parmi lesquelles choisir</param>
        /// <returns></returns>
        [MoonSharpVisible(true)]
        internal Card ChooseBetween(Player player, IEnumerable<Card> cards)
        {
            return _externCallbacks.ExternChooseBetween(player, cards.ToList());
        }

        [MoonSharpVisible(true)]
        internal Card MakeVirtual(string nom, string description, int? imageId = null, Closure? effect = null)
        {
            var makeVirtual = new Card(this, nom, description, imageId ?? 0, effect, _maxId++);
            _allCards.Add(makeVirtual);
            makeVirtual.OnCardCreate();
            return makeVirtual;
        }

        /// <summary>
        /// Fonction lua qui résout une cible
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="effectOwner"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        internal ITargetable LuaAskForTarget(Effect effect, Player effectOwner, int i)
        {
            i -= 1; //parce que lua
            var target = effect.AllTargets[i];
            ITargetable resolved;

            if (target.IsAutomatic)
            {
                try
                {
                    resolved = target.GetAutomaticTarget();
                }
                catch (InvalidEffectException exc)
                {
                    throw new InvalidEffectException(effect, exc.Message, GetScriptByName(effect.EffectId)!);
                }
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

        [MoonSharpVisible(false)]
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

        [MoonSharpVisible(false)]
        public void Log(string source, string message)
        {
            _externCallbacks.DebugPrint("C#", source, message);
        }

        public Card CreateNewCard(Effect effect, bool isVirtual = false, Card? virtualThis = null)
        {
            var newCard = new Card(this, effect, _maxId++, isVirtual, virtualThis);
            _allCards.Add(newCard);
            return newCard;
        }


        public Card CreateNewCard(string name, string description, int imageId, Closure? effect)
        {
            var newCard = new Card(this, name, description, imageId, effect, _maxId++);
            _allCards.Add(newCard);
            return newCard;
        }

        internal int LuaGetRandomNumber(int a, int b)
        {
            return _externCallbacks.GetExternalRandomNumber(a, b);
        }
    }
}