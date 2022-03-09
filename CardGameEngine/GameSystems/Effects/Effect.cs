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
    public class Effect
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

        private void FillCommonGlobals(Game game, Player? effectOwner, object theThis)
        {
            //fonctions
            Script.Globals["SubscribeTo"] =
                (Func<Type, Closure, bool?, bool?, EventManager.IEventHandler>)game.EventManager.LuaSubscribeToEvent;

            Script.Globals["UnsubscribeTo"] =
                (Action<EventManager.IEventHandler>)game.EventManager.LuaUnsubscribeFromEvent;

            Script.Globals["GetRandomNumber"] =
                (Func<int, int, int>)game.LuaGetRandomNumber;

            //propriétés

            if (effectOwner != null) Script.Globals["EffectOwner"] = effectOwner;
            Script.Globals["Game"] = game;
            Script.Globals["This"] = theThis;
            Script.Globals["EventStack"] = game.EventManager.CurrentEventList;

            //types des évenements

            var typeInfos = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace?.StartsWith("CardGameEngine.EventSystem.Events") ?? false)
                .Where(t => typeof(Event).IsAssignableFrom(t));

            foreach (var typeInfo in typeInfos)
            {
                Script.Globals["T_" + typeInfo.Name] = typeInfo;
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
            try
            {
                var method = Script.Globals.Get(methodName).CheckType(nameof(RunMethod), DataType.Function);

                return method.Function.Call(parameters);
            }
            catch (ScriptRuntimeException exception)
            {
                throw new LuaException(exception, exception.CallStack.ToList());
            }
        }

        internal DynValue RunMethodOptional(string methodName, params object[] parameters)
        {
            try
            {
                var dynValue = Script.Globals.Get(methodName);
                return dynValue.Type == DataType.Function ? dynValue.Function.Call(parameters) : DynValue.Nil;
            }
            catch (ScriptRuntimeException exception)
            {
                throw new LuaException(exception, exception.CallStack.ToList());
            }
        }

        public T GetProperty<T>(string propertyName)
        {
            return Script.Globals.Get(propertyName).ToObject<T>();
        }
    }
}