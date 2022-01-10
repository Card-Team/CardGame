using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CardGameEngine.EventSystem;
using CardGameEngine.EventSystem.Events;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Classe représentant un effet
    /// </summary>
    internal class Effect
    {
        /// <summary>
        /// Type de l'effet
        /// </summary>
        internal EffectType EffectType { get; }

        /// <summary>
        /// Nom/référence de l'effet
        /// </summary>
        internal string EffectId { get; }

        /// <summary>
        /// Liste des cibles de l'effet
        /// </summary>
        internal List<Target> AllTargets { get; }

        private Script Script { get; }


        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="effectType">Type de l'effet</param>
        /// <param name="effectId">Nom/référence de l'effet</param>
        /// <param name="targets">Liste des cibles de l'effet</param>
        internal Effect(EffectType effectType, string effectId, List<Target> targets, Script script)
        {
            EffectType = effectType;
            EffectId = effectId;
            AllTargets = targets;
            Script = script;
        }

        private void FillCommonGlobals(Game game, Player effectOwner, object theThis)
        {
            //fonctions
            Script.Globals["SubscribeTo"] =
                (Func<Type, Closure, bool?, bool?, EventManager.IEventHandler>) game.EventManager.LuaSubscribeToEvent;

            Script.Globals["UnsubscribeTo"] =
                (Action<EventManager.IEventHandler>) game.EventManager.LuaUnsubscribeFromEvent;

            //propriétés

            Script.Globals["EffectOwner"] = effectOwner;
            Script.Globals["Game"] = game;
            Script.Globals["This"] = theThis;

            //types des évenements

            var typeInfos = Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(t => string.Equals(t.Namespace, nameof(EventSystem.Events), StringComparison.Ordinal))
                .Where(t => t.IsAssignableFrom(typeof(Event)));

            foreach (var typeInfo in typeInfos)
            {
                Script.Globals[typeInfo.Name] = typeInfo;
            }
        }

        internal void FillGlobals(Game game, Player effectOwner, object theThis, Action<Script> filler)
        {
            FillCommonGlobals(game, effectOwner, theThis);

            filler(Script);
        }

        internal T RunMethod<T>(string methodName, params object[] parameters)
        {
            return RunMethod(methodName, parameters).ToObject<T>();
        }

        internal DynValue RunMethod(string methodName, params object[] parameters)
        {
            var method = Script.Globals.Get(methodName).CheckType(nameof(RunMethod), DataType.Function);

            return method.Function.Call(parameters);
        }

        internal DynValue RunMethodOptional(string methodName, params object[] parameters)
        {
            var dynValue = Script.Globals.Get(methodName);
            return dynValue.Type == DataType.Function ? dynValue.Function.Call(parameters) : DynValue.Nil;
        }

        public T GetProperty<T>(string propertyName)
        {
            return Script.Globals.Get(propertyName).ToObject<T>();
        }
    }
}