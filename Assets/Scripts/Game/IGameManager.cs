using UnityEngine;

namespace ProtectTheCastle.Game
{
    public interface IGameManager
    {
        bool gameInProgress { get; }
        bool pickingPlayers { get; }
        bool pickingTowers { get; }
        bool isPlayer1Turn { get; }

        bool StartGame();
        bool PauseOrResumeGame();
        bool StartPickingPlayers();
        bool FinishPickingPlayers();
        bool StartPickingTowers();
        bool FinishPickingTowers();
        void MovePlayer(GameObject characterToMove);
        void EndTurn();
        bool EndGame(GameObject winner);
    }
}
