using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Effects
{
    public static class EffectChecker
    {
        public static bool CheckEffect(string path, EffectType effectType)
        {
            return effectType switch
            {
                EffectType.Artefact => CheckArtefact(path),
                EffectType.Keyword => CheckKeyword(path),
                EffectType.Card => CheckCard(path),
                _ => throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null) // Ne peut pas arriver
            };
        }

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

            // Required lua elements and their type
            Dictionary<string, DataType> typeCheckReq = new Dictionary<string, DataType>
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

            // Optional lua elements and their type
            Dictionary<string, DataType> typeCheckOpt = new Dictionary<string, DataType>
            {
                {"on_level_change", DataType.Function},
                {"on_game_start", DataType.Function},
            };

            // Check if all required lua elements are the good type
            if (typeCheckReq.Any(keyValuePair => script.Globals.Get(keyValuePair.Key).Type != keyValuePair.Value))
            {
                return false;
            }

            // Check if all optional lua elements are the good type or inexistant
            if (typeCheckOpt.Any(keyValuePair => script.Globals.Get(keyValuePair.Key).Type != keyValuePair.Value &&
                                                 script.Globals.Get(keyValuePair.Key).Type != DataType.Nil))
            {
                return false;
            }

            return true;
        }

        private static bool CheckKeyword(string path)
        {
            throw new NotImplementedException();
        }

        private static bool CheckArtefact(string path)
        {
            throw new NotImplementedException();
        }
    }
}