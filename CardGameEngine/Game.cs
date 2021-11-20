using GameSystems;

namespace CardGameEngine
{
    public class Game
    {
        public Player CurrentPlayerTurn { get; }

        public Player Player1 { get; }

        public Player Player2 { get; }


        public void GameLoop()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public bool CheckHasWon(Player playerToCheck)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void StartPlayerTurn(Player player)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void FinishTurn(Player player)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void PlayCard(Player player, Card card)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void ActivateArtifact(Player player, Artefact artefact)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }
}