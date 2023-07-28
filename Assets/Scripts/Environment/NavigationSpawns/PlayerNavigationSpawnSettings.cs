using System;
using System.Collections.Generic;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    [Serializable]
    public class PlayerNavigationSpawnLevel
    {
        public PlayerNavigationSpawnDirection forest;
    }

    [Serializable]
    public class PlayerNavigationSpawnDirection
    {
        public List<PlayerNavigationSpawnCoordinates> common;
        public List<PlayerNavigationSpawnCoordinates> straight;
        public List<PlayerNavigationSpawnCoordinates> left;
        public List<PlayerNavigationSpawnCoordinates> right;
    }

    [Serializable]
    public class PlayerNavigationSpawnCoordinates
    {
        public float x;
        public float z;
        public bool isDecisionSpawn;
    }
}
