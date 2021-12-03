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


        public void GameLoop()
        {
            throw new System.NotImplementedException();
        }

        public bool CheckHasWon(Player playerToCheck)
        {
            throw new System.NotImplementedException();
        }

        public void StartPlayerTurn(Player player)
        {
            throw new System.NotImplementedException();
        }

        public void FinishTurn(Player player)
        {
            throw new System.NotImplementedException();
        }

        public void PlayCard(Player player, Card card)
        {
            throw new System.NotImplementedException();
        }

        public void ActivateArtifact(Player player, Artefact artefact)
        {
            throw new System.NotImplementedException();
        }
    }
}