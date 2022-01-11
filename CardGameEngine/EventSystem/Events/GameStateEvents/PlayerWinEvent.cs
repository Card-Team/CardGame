﻿using CardGameEngine.GameSystems;

namespace CardGameEngine.EventSystem.Events.GameStateEvents
{
    /// <summary>
    /// Evenement de victoire
    /// </summary>
    public class PlayerWinEvent : Event
    {
        /// <summary>
        /// Joueur ayant gagné
        /// </summary>
        public Player Player { get; }

        internal PlayerWinEvent(Player player)
        {
            Player = player;
        }
    }
}