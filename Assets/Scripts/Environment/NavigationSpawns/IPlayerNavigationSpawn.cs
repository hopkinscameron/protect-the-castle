using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public interface IPlayerNavigationSpawnPoint
    {
        GameObject occupiedBy { get; }
        bool isDecisionSpawn { get; set; }
    }
}
