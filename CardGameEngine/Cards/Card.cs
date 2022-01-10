using System;
using System.Collections.Generic;
using CardGameEngine.EventSystem;
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
        /// Effet de la carte
        /// </summary>
        internal Effect Effect { get; }

        /// <summary>
        /// Mots clé appliqués à la carte
        /// </summary>
        public List<Keyword> Keywords { get; set; }


        internal Card(EventManager evtManager)
        {
            Name = new EventProperty<Card, string, CardNameChangeEvent>(this, evtManager);
            Cost = new EventProperty<Card, int, CardCostChangeEvent>(this, evtManager);
            CurrentLevel = new EventProperty<Card, int, CardLevelChangeEvent>(this, evtManager);
        }


        /// <summary>
        /// Exécute l'effet de la carte
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Un booléen en fonction de la réussite</returns>
        internal bool DoEffect(Game game, Player effectOwner)
        {
            SetUpScriptBeforeRunning(game, effectOwner);

            throw new System.NotImplementedException();
        }

        private void SetUpScriptBeforeRunning(Game game, Player effectOwner)
        {
            Effect.FillGlobals(game, effectOwner, this, script =>
            {
                //globals spécifique au cartes :
                script.Globals["AskForTarget"] =
                    (Func<int, ITargetable>)(i => game.LuaAskForTarget(Effect, effectOwner, i));
            });
        }

        /// <summary>
        /// Vérifie la validité de la précondition de la carte
        /// </summary>
        /// <param name="game">La partie en cours</param>
        /// <returns>Un booléen en fonction de la validité</returns>
        public bool CanBePlayed(Game game,Player effectOwner)
        {
            SetUpScriptBeforeRunning(game,effectOwner);
            throw new System.NotImplementedException();
        }
    }
}