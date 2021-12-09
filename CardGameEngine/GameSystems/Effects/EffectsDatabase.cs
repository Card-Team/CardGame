using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.GameSystems.Effects
{
    public class EffectsDatabase
    {
        private readonly Dictionary<string, Effect> _effectDictionary = new Dictionary<string, Effect>();

        public Effect this[string s] => _effectDictionary[s];

        private void LoadEffect(string path, EffectType effectType)
        {
            string effectId = Path.GetFileNameWithoutExtension(path);

            if (!EffectChecker.CheckEffect(path, effectType))
            {
                throw new InvalidEffectException(effectId, effectType);
            }

            var script = GetDefaultScript();
            script.DoFile(path);

            // Get the targets of the effect
            var targets = script.Globals.Get("targets")
                .Table.Values
                .Select(t => t.UserData.Object)
                .Cast<Target>()
                .ToList();


            _effectDictionary[effectId] = new Effect(effectType, effectId, targets);
        }

        internal static Script GetDefaultScript()
        {
            Script script = new Script
            {
                // Elements c# à intégrer dans le fichier lua
                Globals =
                {
                    ["CreateTarget"] = (Func<string, TargetTypes, bool, Closure?, Target>) CreateTarget,
                    ["TargetTypes"] = UserData.CreateStatic<TargetTypes>(),
                }
            };
            return script;
        }

        public void LoadAllEffects(string path)
        {
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                var validFile = Enum.TryParse(Path.GetFileName(directory), out EffectType type);
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