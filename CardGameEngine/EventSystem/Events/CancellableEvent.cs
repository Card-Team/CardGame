﻿using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events
{
    /// <summary>
    /// Classe abstraite représentant les évènement pouvant être annulés
    /// </summary>
    public abstract class CancellableEvent : Event
    {
        /// <summary>
        /// Booléen indiquant l'annulation de l'évènement
        /// </summary>
        internal bool Cancelled { get; set; }
    }
}