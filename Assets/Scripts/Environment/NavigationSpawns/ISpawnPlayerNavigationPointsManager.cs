using ProtectTheCastle.Players.Enums;
using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public interface ISpawnPlayerNavigationPointsManager
    {
        void Load();
        GameObject GetHomeBaseSpawn(GameObject homeBase);
        GameObject GetNextTarget(GameObject homeBase, Vector3 currentPosition, EnumPlayerMoveDirection directionToMove);
        GameObject GetNextTarget(GameObject homeBase, GameObject currentTarget, EnumPlayerMoveDirection directionToMove);
    }
}
