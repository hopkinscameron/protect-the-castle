using System;

namespace ProtectTheCastle.Game
{
    [Serializable]
    public class PlayerPrefabSettings 
    {
        public PlayerPrefabTypeSettings normal;
        public PlayerPrefabTypeSettings hero;
        public PlayerPrefabTypeSettings soldier;
        public PlayerPrefabTypeSettings mercenary;
        public PlayerPrefabTypeSettings warrior;
        public PlayerPrefabTypeSettings magic;
    }

    [Serializable]
    public class PlayerPrefabTypeSettings 
    {
        public float damage;
        public float health;
        public float speed;
    }
}
