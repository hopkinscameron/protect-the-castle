using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public interface IPlayerNavigationSpawnPoint
    {
        GameObject occupiedBy { get; }
        bool isDecisionSpawn { get; set; }
        bool isPlayer1WinCondition { get; set; }
        bool isPlayer2WinCondition { get; set; }
    }
}
