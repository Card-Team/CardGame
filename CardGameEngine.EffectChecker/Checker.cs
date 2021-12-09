using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;

namespace CardGameEngine.EffectChecker
{
    class Checker
    {
        public bool CheckEffect(string content)
        {
            Script script = new Script
            {
                // Elements c# à intégrer dans le fichier lua
                Globals =
                {
                    ["CreateTarget"] = (Func<string, TargetTypes, bool, Closure?, object>) CreateTarget,
                    ["TargetTypes"] = UserData.CreateStatic<TargetTypes>(),
                }
            };

            try
            {
                script.DoString(content);
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
            return !typeCheckOpt.Any(keyValuePair => script.Globals.Get(keyValuePair.Key).Type != keyValuePair.Value &&
                                                     script.Globals.Get(keyValuePair.Key).Type != DataType.Nil);
        }

        private static object CreateTarget(string osef, TargetType osef2, bool osef3, Closure? osef4)
        {
            return null;
        }
    }

    internal enum TargetTypes
    {
        Player,
        Card
    }
}