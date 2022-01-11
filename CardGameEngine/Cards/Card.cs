using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.EventSystem.Events.CardEvents.PropertyChange;
using CardGameEngine.GameSystems;
using CardGameEngine.GameSystems.Effects;
using CardGameEngine.GameSystems.Targeting;

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
        /// Effet de la carte
        /// </summary>
        internal Effect Effect { get; }

        /// <summary>
        /// Mots clé appliqués à la carte
        /// </summary>
        public List<Keyword> Keywords { get; }

        public bool IsMaxLevel => CurrentLevel.Value == MaxLevel;

        private readonly Game _game;


        internal Card(Game game, Effect effect)
        {
            if (effect.EffectType != EffectType.Card)
                throw new InvalidOperationException(
                    $"Tentative de création d'une carte à partir de l'effet {effect} alors que son type est de {effect.EffectType}");
            _game = game;

            Effect = effect;

            Name = new EventProperty<Card, string, CardNameChangeEvent>(this, game.EventManager,
                effect.GetProperty<string>(LuaStrings.Card.NameProperty));

            Cost = new EventProperty<Card, int, CardCostChangeEvent>(this, game.EventManager,
                effect.GetProperty<int>(LuaStrings.Card.CostProperty));

            MaxLevel = effect.GetProperty<int>(LuaStrings.Card.MaxLevelProperty);

            Description = new EventProperty<Card, string, CardDescriptionChangeEvent>(this, game.EventManager,
                effect.GetProperty<string>(LuaStrings.Card.DescriptionProperty));

            CurrentLevel = new EventProperty<Card, int, CardLevelChangeEvent>(this, game.EventManager, 1);

            Keywords = new List<Keyword>();
        }


        /// <summary>
        /// Exécute l'effet de la carte
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Un booléen en fonction de la réussite</returns>
        internal bool DoEffect(Game game, Player effectOwner)
        {
            var effectActivateEvent = new EffectActivateEvent();
            SetUpScriptBeforeRunning(game, effectOwner);
            Effect.RunMethod(LuaStrings.Card.DoEffectMethod);
            return true;
        }

        //todo voir comment l'appeller dans les evenements
        private void SetUpScriptBeforeRunning(Game game, Player effectOwner)
        {
            Effect.FillGlobals(game, effectOwner, this, script =>
            {
                //globals spécifique au cartes :
                script.Globals["AskForTarget"] =
                    (Func<int, ITargetable>) (i => game.LuaAskForTarget(Effect, effectOwner, i));

                script.Globals["TargetsExists"] =
                    (Func<List<int>, bool>) (list => game.LuaTargetsExists(Effect, effectOwner, list));
            });
        }

        /// <summary>
        /// Vérifie la validité de la précondition de la carte
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Un booléen en fonction de la validité</returns>
        public bool CanBePlayed(Game game, Player effectOwner)
        {
            SetUpScriptBeforeRunning(game, effectOwner);
            return Effect.RunMethod<bool>("precondition");
        }

        public bool Upgrade()
        {
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
            Effect.RunMethodOptional(LuaStrings.Card.OnCardCreateMethod);
        }

        public override string ToString()
        {
            return $"{Name.Value} : Lvl {CurrentLevel.Value}/{MaxLevel}, Cost {Cost.Value}";
        }
    }
}