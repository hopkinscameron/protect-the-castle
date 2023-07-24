using System;

namespace ProtectTheCastle.Game
{
    [Serializable]
    public class PlayerPrefabSettings 
    {
        public PlayerPrefabTypeSettings normal;
        public PlayerPrefabTypeSettings fire;
        public PlayerPrefabTypeSettings grass;
        public PlayerPrefabTypeSettings water;
        public PlayerPrefabTypeSettings ice;
        public PlayerPrefabTypeSettings poison;
    }

    [Serializable]
    public class PlayerPrefabTypeSettings 
    {
        public float damage;
        public float health;
        public float speed;
    }
}
