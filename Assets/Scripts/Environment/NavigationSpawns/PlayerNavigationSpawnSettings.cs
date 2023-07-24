using System;
using System.Collections.Generic;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    [Serializable]
    public class PlayerNavigationSpawnSettings 
    {
        public List<PlayerNavigationSpawn> straight;
        public List<PlayerNavigationSpawn> left;
        public List<PlayerNavigationSpawn> right;
    }

    [Serializable]
    public class PlayerNavigationSpawn 
    {
        public float x;
        public float z;
    }
}
