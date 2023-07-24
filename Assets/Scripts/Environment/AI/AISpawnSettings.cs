using System;
using System.Collections.Generic;

namespace ProtectTheCastle.Environment.AISpawns
{
    [Serializable]
    public class AISpawnSettings
    {
        public List<AISpawn> forest;
    }

    [Serializable]
    public class AISpawn
    {
        public float x;
        public float z;
    }
}
