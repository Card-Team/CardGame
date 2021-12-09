﻿using System;
using System.IO;
using CardGameEngine.GameSystems.Effects;
using CardGameEngine.GameSystems.Targeting;
using NUnit.Framework;

namespace CardGameTests
{
    [TestFixture(Author = "Bilel",
        Category = "Effets",
        TestOf = typeof(EffectsDatabase),
        TestName = "Tests du chargement des effets")]
    public class Tests
    {
        private EffectsDatabase _effectsDatabase = null!;

        private readonly string _randomDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0]);

        [SetUp]
        public void SetUp()
        {
            Directory.CreateDirectory(_randomDir);
            _effectsDatabase = new EffectsDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_randomDir, true);
        }


        private string PutScript(string script,EffectType type)
        {
            var typePath = Path.Combine(_randomDir, type.ToString());
            Directory.CreateDirectory(typePath);
            
            var filePath = Path.Combine(typePath, Path.GetRandomFileName());
            using var f = new StreamWriter(File.Create(filePath));
            f.Write(script);

            return filePath;
        }


        
        [Test(Description = "Verifier que les effets bien formés sont acceptés")]
        [TestCaseSource(typeof(LuaTestData),nameof(LuaTestData.GetAllTestDataOfType)
            ,new object[]{LuaTestData.TestScriptType.Good})]
        public void Test_Load_Effect_Good(EffectType type,string script)
        {
            var path = PutScript(script,type);
            Console.WriteLine(script);
            Assert.DoesNotThrow(() => _effectsDatabase.LoadAllEffects(_randomDir)
                , "Les effets sont bien formés et ne doivent pas être rejetés");
        }

        [Test(Description = "Verifier que les effets mal formés sont rejetés")]
        [TestCaseSource(typeof(LuaTestData),nameof(LuaTestData.GetAllTestDataOfType)
            ,new object[]{LuaTestData.TestScriptType.Bad})]
        public void Test_Load_Effect_Bad(EffectType type,string script)
        {
            var path = PutScript(script,type);
            Console.WriteLine(script);
            Assert.Throws<InvalidEffectException>(() => _effectsDatabase.LoadAllEffects(_randomDir)
                , "Les effets sont invalides et doivent être rejetés");
        }

        [Test(Description = "Verifier que le chargement d'une carte contient bien les données attendues")]
        [TestCaseSource(typeof(LuaTestData),nameof(LuaTestData.GetNamedTestData),
            new object[]{LuaTestData.TestScriptType.Good,EffectType.Card,"example.lua"})]
        public void Test_Effect_Card_Data(EffectType type,string scriptContent)
        {
            var path = Path.GetFileNameWithoutExtension(PutScript(scriptContent,type));
            _effectsDatabase.LoadAllEffects(_randomDir);
            var eft = _effectsDatabase[path];
            Assert.That(eft,Is.Not.Null);

            Assert.That(eft.EffectType,Is.EqualTo(EffectType.Card));
            Assert.That(eft.EffectId,Is.EqualTo(path));

   
            Assert.That(eft.AllTargets,Is.Not.Null.And.Count.EqualTo(2).And.All.Not.Null);
   
            Assert.That(eft.AllTargets,Is.Unique);
            
            Assert.That(eft.AllTargets,Has.Exactly(1)
                .With.Property(nameof(Target.Name))
                .EqualTo("Une cible carte")
                .And
                .Property(nameof(Target.TargetType))
                .EqualTo(TargetTypes.Card)
                .And
                .Property(nameof(Target.IsAutomatic))
                .False
            );

            Assert.That(eft.AllTargets,Has.Exactly(1)
                .With.Property(nameof(Target.Name))
                .EqualTo("Un joueur")
                .And
                .Property(nameof(Target.TargetType))
                .EqualTo(TargetTypes.Player)
                .And
                .Property(nameof(Target.IsAutomatic))
                .True
            );
            
        }
    }
}