﻿using System;
using System.Collections.Generic;
using System.IO;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.GameSystems.Effects
{
    public class EffectsDatabase
    {
        private Dictionary<string, Effect> _effectDictionary = new Dictionary<string, Effect>();

        public Effect this[string s] => _effectDictionary[s];

        private void LoadEffect(string path, EffectType effectType)
        {
            Script script = new Script
            {
                Globals =
                {
                    ["CreateTarget"] = (Func<string, TargetTypes, bool, Closure?, Target>) CreateTarget,
                    // ["TargetTypes"] = typeof(TargetTypes),
                }
            };

            script.DoFile(path);

            Dictionary<string, DataType> typeCheck = new Dictionary<string, DataType>
            {
                {"max_level", DataType.Number},
                {"image_id", DataType.Number},
                {"name", DataType.String},
                {"pa_cost", DataType.Number},
                {"targets", DataType.Table},
                {"precondition", DataType.Function},
                {"description", DataType.Function},
                {"do_effect", DataType.Function},
            };

            foreach (var keyValuePair in typeCheck)
            {
                if (script.Globals.Get(keyValuePair.Key).Type != keyValuePair.Value)
                {
                    throw new InvalidEffectException(path, effectType);
                }
            }

            if (script.Globals.Get("on_level_change").Type == DataType.Function ||
                script.Globals.Get("on_level_change").Type == DataType.Nil)
            {
                throw new InvalidEffectException(path, effectType);
            }

            string[] split = path.Split('\\');
            string effectId = split[split.Length - 1];

            //TODO Trouver la liste des cibles
            //return new Effect(effectType, effectId, new List<Target>());
        }

        public void LoadAllEffects(string path)
        {
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                var validFile = Enum.TryParse(Path.GetDirectoryName(directory), out EffectType type);
                if (validFile)
                    LoadAllEffects(directory, type);
            }
        }

        private void LoadAllEffects(string path, EffectType effectType)
        {
            foreach (var file in Directory.EnumerateFiles(path))
            {
                LoadEffect(file, effectType);
            }
        }

        private static Target CreateTarget(string targetName, TargetTypes targetType, bool isAutomatic,
            Closure? cardFilter = null)
        {
            return new Target(targetName, targetType, isAutomatic, cardFilter);
        }
    }
}