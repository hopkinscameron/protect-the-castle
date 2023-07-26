using System;
using System.Collections.Generic;

namespace ProtectTheCastle.Environment.TowerSpawns
{
    [Serializable]
    public class TowerSpawnSettings
    {
        public List<TowerSpawn> forest;
    }

    [Serializable]
    public class TowerSpawn
    {
        public float x;
        public float z;
        public float rotation;
    }
}
