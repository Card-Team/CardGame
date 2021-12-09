using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CardGameEngine.GameSystems.Effects;
using NUnit.Framework;

namespace CardGameTests
{
    [ExcludeFromCodeCoverage]
    public class LuaTestData
    {
        private const string ScriptFolder = "SampleScripts";

        public enum TestScriptType
        {
            Bad,
            Good
        }

        public static IEnumerable<TestCaseData> GetNamedTestData(EffectType effectType, TestScriptType scriptType,
            string name)
        {
            string scriptFile = Path.Combine(ScriptFolder, effectType.ToString(), scriptType.ToString(), name);

            if (!File.Exists(scriptFile))
                Assert.Inconclusive($"Script not found with path {scriptFile}");

            return Enumerable.Repeat(BuildTestCase(effectType, scriptFile), 1);
        }

        private static TestCaseData BuildTestCase(EffectType effectType, string scriptFile)
        {
            var text = File.ReadAllText(scriptFile);
            return new TestCaseData(effectType, text)
                .SetName(CamelToSpaces(Path.GetFileNameWithoutExtension(scriptFile)))
                .SetCategory(effectType.ToString());
        }

        public static IEnumerable<TestCaseData> GetAllTestDataOfType(TestScriptType scriptType)
        {
            return GetAllEffectTestData(EffectType.Card, scriptType);
            //.Concat(GetAllEffectTestData(EffectType.Artefact,scriptType))
            //.Concat(GetAllEffectTestData(EffectType.Keyword,scriptType));
        }


        public static IEnumerable<TestCaseData> GetAllEffectTestData(EffectType effectType, TestScriptType scriptType)
        {
            string scriptFolder = Path.Combine(ScriptFolder, effectType.ToString(), scriptType.ToString());

            if (!Directory.Exists(scriptFolder))
            {
                Assert.Inconclusive($"Script Folder directory not found at {scriptFolder}");
            }

            foreach (string file in Directory.EnumerateFiles(scriptFolder))
                yield return GetNamedTestData(effectType, scriptType, Path.GetFileName(file)).First();
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