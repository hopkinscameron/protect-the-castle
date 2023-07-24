using System;
using System.Collections.Generic;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    [Serializable]
    public class PlayerNavigationSpawnSettings 
    {
        public List<PlayerNavigationSpawnCoordinates> straight;
        public List<PlayerNavigationSpawnCoordinates> left;
        public List<PlayerNavigationSpawnCoordinates> right;
    }

    [Serializable]
    public class PlayerNavigationSpawnCoordinates
    {
        public float x;
        public float z;
    }
}
