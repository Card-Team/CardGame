using System;
using CardGameEngine.Cards;
using CardGameEngine.EventSystem;
using CardGameEngine.GameSystems;

namespace CardGameEngine
{
    public class Game
    {
        public Player CurrentPlayerTurn { get; }

        public Player Player1 { get; }

        public Player Player2 { get; }

        public EventManager EventManager { get; }

        public EffectsDatabase EffectsDatabase { get; }
        

        public bool CheckHasWon(Player playerToCheck)
        {
            throw new NotImplementedException();
        }

        public void StartPlayerTurn(Player player)
        {
            throw new NotImplementedException();
        }

        public void FinishTurn(Player player)
        {
            throw new NotImplementedException();
        }

        public bool CanBePlayed(Player by, Card Card)
        {
            throw new NotImplementedException();
        }

        public void PlayCard(Player player, Card card)
        {
            throw new NotImplementedException();
        }

        public void ActivateArtifact(Player player, Artefact artefact)
        {
            throw new NotImplementedException();
        }
    }
}