using System;
using System.Collections.Generic;
using CardGameEngine.Cards;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Classe statique ayant pour but de vérifier la validité d'un script d'effet lua
    /// </summary>
    /// <seealso cref="Effect"/>
    internal static class EffectChecker
    {
        /// <summary>
        /// Méthode principale de la classe, elle appelle la méthode correspondante au type d'effet donné.
        /// </summary>
        /// <param name="path">Nom complet du fichier de l'effet</param>
        /// <param name="effectType">Type d'effet</param>
        /// <returns>Un booléen en fonction de la validité de l'effet</returns>
        /// <seealso cref="CheckArtefact(string)"/>
        /// <seealso cref="CheckCard(string)"/>
        /// <seealso cref="CheckKeyword(string)"/>
        internal static bool CheckEffect(string path, EffectType effectType)
        {
            return effectType switch
            {
                EffectType.Artefact => true,
                EffectType.Card => CheckCard(path),
                EffectType.Keyword => true,
                _ => throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null) // Ne peut pas arriver
            };
        }

        /// <summary>
        /// Méthode pour tester la validité de l'effet d'une carte
        /// </summary>
        /// <param name="path">Nom complet du fichier de la carte</param>
        /// <returns>Un booléen en fonction de la validité</returns>
        private static bool CheckCard(string path)
        {
            Script script = EffectsDatabase.GetDefaultScript();

            try
            {
                script.DoFile(path);
            }
            catch (ScriptRuntimeException sre)
            {
                Console.WriteLine($"Erreur à l'exécution de {path}");
                Console.WriteLine(sre.DecoratedMessage);
                return false;
            }
            catch (SyntaxErrorException see)
            {
                Console.WriteLine($"Erreur de syntaxe dans {path}");
                Console.WriteLine(see.DecoratedMessage);
                return false;
            }

            // Élements lua requis et leur type respectif
            Dictionary<string, DataType> typeCheckReq = new Dictionary<string, DataType>
            {
                {LuaStrings.Card.MaxLevelProperty, DataType.Number},
                {LuaStrings.Card.ImageIdProperty, DataType.Number},
                {LuaStrings.Card.NameProperty, DataType.String},
                {LuaStrings.Card.CostProperty, DataType.Number},
                {LuaStrings.Card.TargetsProperty, DataType.Table},
                {LuaStrings.Card.DescriptionProperty, DataType.String},
                {LuaStrings.Card.PreconditionMethod, DataType.Function},
                {LuaStrings.Card.DoEffectMethod, DataType.Function},
            };

            // Élements lua optionnels et leur type respectif
            Dictionary<string, DataType> typeCheckOpt = new Dictionary<string, DataType>
            {
                {LuaStrings.Card.OnLevelChangeMethod, DataType.Function},
                {LuaStrings.Card.OnCardCreateMethod, DataType.Function},
            };

            // Vérifie si tous les élements requis sont existants et du bon type
            foreach (var keyValuePair in typeCheckReq)
            {
                var key = script.Globals.Get(keyValuePair.Key);
                if (key.Type != keyValuePair.Value)
                {
                    return false;
                }

                if (key.Type == DataType.Number && (double.IsNaN(key.Number) || double.IsInfinity(key.Number)))
                {
                    return false;
                }
            }

            // Vérifie si tous les élements optionnels du bon type ou inexistants
            foreach (var keyValuePair in typeCheckOpt)
            {
                var key = script.Globals.Get(keyValuePair.Key);
                if (key.Type != keyValuePair.Value && key.Type != DataType.Nil)
                {
                    return false;
                }

                if (key.Type == DataType.Number && (double.IsNaN(key.Number) || double.IsInfinity(key.Number)))
                {
                    return false;
                }
            }

            // Aucun problème trouvé, effet validé
            return true;
        }

        /// <summary>
        /// Méthode pour tester la validité de l'effet d'un mot clé
        /// </summary>
        /// <param name="path">Nom complet du fichier du mot clé</param>
        /// <returns>Un booléen en fonction de la validité</returns>
        /// <seealso cref="Keyword"/>
        private static bool CheckKeyword(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Méthode pour tester la validité de l'effet d'un artefact
        /// </summary>
        /// <param name="path">Nom complet du fichier de l'artefact</param>
        /// <returns>Un booléen en fonction de la validité</returns>
        /// <seealso cref="Artefact"/>
        private static bool CheckArtefact(string path)
        {
            throw new NotImplementedException();
        }
    }
}