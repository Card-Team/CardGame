using System.Collections.Generic;
using CardGameEngine.GameSystems.Targeting;
using MoonSharp.Interpreter;

namespace CardGameEngine.GameSystems.Effects
{
    /// <summary>
    /// Classe représentant un effet
    /// </summary>
    internal class Effect
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

        internal Script Script { get; }


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
    }
}