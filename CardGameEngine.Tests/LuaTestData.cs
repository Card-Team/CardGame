using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CardGameEngine.GameSystems.Effects;
using NUnit.Framework;

namespace CardGameTests
{
    public class LuaTestData
    {
        private const string ScriptFolder = "SampleScripts";

        public enum TestScriptType
        {
            Bad,
            Good
        }

        public static IEnumerable<TestCaseData> GetNamedTestData(TestScriptType scriptType, EffectType effectType, string name)
        {
            string scriptFile = Path.Combine(ScriptFolder,effectType.ToString(),scriptType.ToString(), name);
            if (File.Exists(scriptFile))
            {
                yield return BuildTestCase(effectType, scriptFile);
            }
            else throw new ArgumentException($"Script not fund with path {scriptFile}");
        }

        private static TestCaseData BuildTestCase(EffectType effectType, string scriptFile)
        {
            var text = File.ReadAllText(scriptFile);
            return new TestCaseData(effectType,text)
                .SetName(CamelToSpaces(Path.GetFileNameWithoutExtension(scriptFile)))
                .SetCategory(effectType.ToString());
        }

        public static IEnumerable<TestCaseData> GetAllTestDataOfType(TestScriptType scriptType)
        {
            return GetAllEffectTestData(null, scriptType,null);
        }

        public static IEnumerable<TestCaseData> GetAllEffectTestData(EffectType? effectType,TestScriptType scriptType,string? name)
        {
            foreach (string currentTypeFolder in Directory.EnumerateDirectories(ScriptFolder)
                .Where(e => effectType == null || Path.GetFileName(e) == effectType.ToString()))
            {
                string scriptFolder = Path.Combine(currentTypeFolder, scriptType.ToString());
                
                if (!Directory.Exists(scriptFolder)) continue;
                var curEffectType = (EffectType)Enum.Parse(typeof(EffectType), Path.GetFileName(currentTypeFolder));
                
                foreach (string file in Directory.EnumerateFiles(scriptFolder)
                    .Where(f => name == null || Path.GetFileName(f) == name))
                    yield return BuildTestCase(curEffectType, file);
            }
        }

        private static string CamelToSpaces(string str)
        {
            //Add a space between each lower case character and upper case character
            var replace = Regex.Replace(str, "([a-z])([A-Z])", "$1 $2").Trim();
            replace = replace[0].ToString().ToUpper() + replace.Substring(1);
            return replace;
        }
    }
}