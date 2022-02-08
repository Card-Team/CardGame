using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;
using CardGameEngine.GameSystems;
using CardGameEngine.GameSystems.Effects;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.Cards
{
    /// <summary>
    /// Classe représentant une carte
    /// </summary>
    public class Card : ITargetable
    {
        /// <summary>
        /// Nom de la carte
        /// </summary>
        public EventProperty<Card, string, CardNameChangeEvent> Name { get; }

        public string EffectId => Effect.EffectId;

        /// <summary>
        /// Est-ce que la carte est virtuelle ?
        /// </summary>
        public bool IsVirtual { get; internal set; }

        /// <summary>
        /// Niveau maximum de la carte
        /// </summary>
        public int MaxLevel { get; }

        /// <summary>
        /// Coût de la carte (ActionPoints du player)
        /// </summary>
        public EventProperty<Card, int, CardCostChangeEvent> Cost { get; }

        /// <summary>
        /// Niveau de la carte
        /// </summary>
        public EventProperty<Card, int, CardLevelChangeEvent> CurrentLevel { get; }

        /// <summary>
        /// Description de la carte
        /// </summary>
        public EventProperty<Card, string, CardDescriptionChangeEvent> Description { get; }

        /// <summary>
        /// Id de l'image de la carte
        /// </summary>
        private EventProperty<Card, int, CardImageIdChangeEvent> ImageId { get; }

        /// <summary>
        /// Effet de la carte
        /// </summary>
        internal Effect Effect { get; }

        /// <summary>
        /// Mots clé appliqués à la carte
        /// </summary>
        private List<Keyword> Keywords { get; }

        /// <summary>
        /// Id de la carte dans la partie
        /// </summary>
        private int Id { get; }

        public bool IsMaxLevel => CurrentLevel.Value == MaxLevel;

        private readonly Game _game;
        private readonly Closure? _virtualClosure;

        [MoonSharpVisible(true)]
        internal Card Virtual()
        {
            Card virt = _game.CreateNewCard(_game.EffectsDatabase[Effect.EffectId](), true);

            virt.OnCardCreate();

            return virt;
        }

        [MoonSharpVisible(true)]
        public Card Clone()
        {
            Card clone = _game.CreateNewCard(_game.EffectsDatabase[Effect.EffectId](), false);

            clone.OnCardCreate();

            return clone;
        }


        internal Card(Game game, Effect effect, int id, bool isVirtual = false)
        {
            if (effect.EffectType != EffectType.Card)
                throw new InvalidOperationException(
                    $"Tentative de création d'une carte à partir de l'effet {effect} alors que son type est de {effect.EffectType}");
            _game = game;

            Effect = effect;

            Id = id;

            IsVirtual = isVirtual;

            Name = new EventProperty<Card, string, CardNameChangeEvent>(this, game.EventManager,
                effect.GetProperty<string>(LuaStrings.Card.NameProperty));

            Cost = new EventProperty<Card, int, CardCostChangeEvent>(this, game.EventManager,
                effect.GetProperty<int>(LuaStrings.Card.CostProperty));

            MaxLevel = effect.GetProperty<int>(LuaStrings.Card.MaxLevelProperty);

            Description = new EventProperty<Card, string, CardDescriptionChangeEvent>(this, game.EventManager,
                effect.GetProperty<string>(LuaStrings.Card.DescriptionProperty));

            ImageId = new EventProperty<Card, int, CardImageIdChangeEvent>(this, game.EventManager,
                effect.GetProperty<int>(LuaStrings.Card.ImageIdProperty));

            CurrentLevel = new LevelEventProperty(this, game.EventManager, 1);

            Keywords = new List<Keyword>();
        }

        internal Card(Game game, string name, string description, int imageId, Closure? effect, int id) : this(game,
            game.EffectsDatabase.Blank(), id, true)
        {
            Name.StealthChange(name);
            Description.StealthChange(description);
            _virtualClosure = effect;
        }


        /// <summary>
        /// Exécute l'effet de la carte
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Vrai si la carte doit etre défaussée, faux sinon</returns>
        internal bool DoEffect(Player effectOwner)
        {
            SetUpScriptBeforeRunning(effectOwner);
            DynValue result;
            if (IsVirtual)
                result = _virtualClosure?.Call() ?? DynValue.Nil;
            else
                result = Effect.RunMethod(LuaStrings.Card.DoEffectMethod);


            if (result.Type == DataType.Boolean) return result.Boolean;

            return true;
        }

        private void SetUpScriptBeforeRunning(Player? effectOwner)
        {
            if (IsVirtual) return;
            Effect.FillGlobals(_game, effectOwner, this, script =>
            {
                if (effectOwner != null)
                {
                    //globals spécifique au cartes :
                    script.Globals["AskForTarget"] =
                        (Func<int, ITargetable>) (i => _game.LuaAskForTarget(Effect, effectOwner, i));

                    script.Globals["TargetsExists"] =
                        (Func<List<int>, bool>) (list => _game.LuaTargetsExists(Effect, effectOwner, list));
                }
            });
        }

        /// <summary>
        /// Vérifie la validité de la précondition de la carte
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Un booléen en fonction de la validité</returns>
        public bool CanBePlayed(Player effectOwner)
        {
            SetUpScriptBeforeRunning(effectOwner);
            return IsVirtual || Effect.RunMethod<bool>(LuaStrings.Card.PreconditionMethod);
        }

        public bool Upgrade()
        {
            if (IsVirtual) return false;
            if (CurrentLevel.Value == MaxLevel)
            {
                throw new InvalidOperationException(
                    $"Tentative d'upgrade de la carte {this} alors qu'elle est a son niveau maximum ({MaxLevel})");
            }

            var newLevel = CurrentLevel.Value + 1;
            CurrentLevel.TryChangeValue(newLevel);
            return newLevel == CurrentLevel.Value;
        }

        internal void OnCardCreate()
        {
            SetUpScriptBeforeRunning(null);
            Effect.RunMethodOptional(LuaStrings.Card.OnCardCreateMethod);
        }

        public override string ToString()
        {
            return $"{Name.Value} :" +
                   (IsVirtual ? $" {Description.Value}" : $" Lvl {CurrentLevel.Value}/{MaxLevel}, Cost {Cost.Value}");
        }

        public void OnLevelChange(int oldLevel, int newLevel)
        {
            SetUpScriptBeforeRunning(null);
            Effect.RunMethodOptional(LuaStrings.Card.OnLevelChangeMethod, oldLevel, newLevel);
        }

        public override bool Equals(object obj)
        {
            return obj is Card card && (Id == card.Id);
        }
    }
}