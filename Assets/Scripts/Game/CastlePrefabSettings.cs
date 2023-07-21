namespace ProtectTheCastle.Game
{
    [System.Serializable]
    public class CastlePrefabSettings 
    {
        public CastlePrefabTypeSettings normal;
        public CastlePrefabTypeSettings heavyArmored;
        public CastlePrefabTypeSettings speedDemon;
        public CastlePrefabTypeSettings medic;
    }

    [System.Serializable]
    public class CastlePrefabTypeSettings 
    {
        public float health;
        public float numCastles;
        public float numPlayers;
    }
}
