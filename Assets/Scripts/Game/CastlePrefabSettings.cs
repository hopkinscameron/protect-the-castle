using System;

namespace ProtectTheCastle.Game
{
    [Serializable]
    public class CastlePrefabSettings 
    {
        public CastlePrefabTypeSettings normal;
        public CastlePrefabTypeSettings heavyArmored;
        public CastlePrefabTypeSettings speedDemon;
        public CastlePrefabTypeSettings medic;
    }

    [Serializable]
    public class CastlePrefabTypeSettings 
    {
        public float health;
        public float numCastles;
        public float numPlayers;
    }
}
