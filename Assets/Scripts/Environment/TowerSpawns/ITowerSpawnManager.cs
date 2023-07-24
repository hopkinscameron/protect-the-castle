using UnityEngine;

namespace ProtectTheCastle.Environment.TowerSpawns
{
    public interface ITowerSpawnManager
    {
        void Load();
        GameObject Spawn(GameObject spawnObject);
    }
}
