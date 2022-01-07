using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Classe lisant, vérifiant et stockant tous les effets valides
    /// </summary>
    internal class EffectsDatabase
    {
        /// <summary>
        /// Le dictionnaire stockant les effets valides avec leur nom comme clé
        /// </summary>
        private readonly Dictionary<string, Effect> _effectDictionary = new Dictionary<string, Effect>();

        /// <summary>
        /// Accède au dictionnaire des effets de l'objet
        /// </summary>
        /// <param name="s">Nom de l'effet</param>
        internal Effect this[string s] => _effectDictionary[s];


        /// <summary>
        /// Méthode principale de la classe, elle permet de charger tous les effets
        /// Redirige vers l'autre méthode LoadAllEffects en fonction du type des effets du sous-dossier
        /// </summary>
        /// <param name="path">Nom complet du dossier</param>
        /// <seealso cref="LoadAllEffects(string, EffectType)"/>
        internal void LoadAllEffects(string path)
        {
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                var validFile = Enum.TryParse(Path.GetFileName(directory), out EffectType type);
                if (validFile)
                    LoadAllEffects(directory, type);
            }
        }

        /// <summary>
        /// Méthode chargeant tous les effets contenus dans un dossier
        /// </summary>
        /// <param name="path">Nom complet du dossier</param>
        /// <param name="effectType">Type de l'effet (correspond aussi au nom du dossier)</param>
        /// <seealso cref="LoadEffect(string, EffectType)"/>
        private void LoadAllEffects(string path, EffectType effectType)
        {
            foreach (var file in Directory.EnumerateFiles(path))
            {
                LoadEffect(file, effectType);
            }
        }

        /// <summary>
        /// Méthode lisant et vérifiant la validité d'un script puis le stocke
        /// </summary>
        /// <param name="path">Nom complet du fichier</param>
        /// <param name="effectType">Type de l'effet</param>
        /// <exception cref="InvalidEffectException">Si l'effet est invalide</exception>
        /// <seealso cref="EffectChecker"/>
        private void LoadEffect(string path, EffectType effectType)
        {
            // ID de l'effet (le nom du fichier sans extension)
            string effectId = Path.GetFileNameWithoutExtension(path);

            // Teste la validité de l'effet
            if (!EffectChecker.CheckEffect(path, effectType))
            {
                throw new InvalidEffectException(effectId, effectType);
            }

            // Charge le script de l'effet
            var script = GetDefaultScript();
            script.DoFile(path);

            // Récupère les cibles de l'effet
            var targets = script.Globals.Get("targets")
                .Table.Values
                .Select(t => t.UserData.Object)
                .Cast<Target>()
                .ToList();

            // Enregistre l'effet dans le dictionnaire
            _effectDictionary[effectId] = new Effect(effectType, effectId, targets);
        }

        /// <summary>
        /// Méthode simple permettant de récupérer le script avec l'intégration du c# nécessaire
        /// </summary>
        /// <returns>L'objet script créé</returns>
        internal static Script GetDefaultScript()
        {
            Script script = new Script
            {
                // Élements c# à intégrer dans le fichier lua
                Globals =
                {
                    ["CreateTarget"] = (Func<string, TargetTypes, bool, Closure?, Target>) CreateTarget,
                    ["TargetTypes"] = UserData.CreateStatic<TargetTypes>(),
                }
            };
            return script;
        }

        /// <summary>
        /// Méthode de création d'un objet Target
        /// </summary>
        /// <returns>Un objet Target</returns>
        /// <seealso cref="Target(string, TargetTypes, bool, Closure?)"/>
        private static Target CreateTarget(string targetName, TargetTypes targetType, bool isAutomatic,
            Closure? cardFilter = null)
        {
            return new Target(targetName, targetType, isAutomatic, cardFilter);
        }
    }
}