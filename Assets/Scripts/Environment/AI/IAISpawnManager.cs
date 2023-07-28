using System.Collections.Generic;
using UnityEngine;

namespace ProtectTheCastle.Environment.AISpawns
{
    public interface IAISpawnManager
    {
        void Load();
        IReadOnlyList<GameObject> SpawnSome();
        GameObject GetNextTarget(Vector3 currentPosition);
        GameObject GetNextTarget(GameObject currentTarget);
    }
}
