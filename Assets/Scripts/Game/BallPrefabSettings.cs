using System;

namespace ProtectTheCastle.Game
{
    [Serializable]
    public class BallPrefabSettings 
    {
        public BallPrefabTypeSettings normal;
        public BallPrefabTypeSettings fire;
        public BallPrefabTypeSettings grass;
        public BallPrefabTypeSettings water;
        public BallPrefabTypeSettings ice;
        public BallPrefabTypeSettings poison;
    }

    [Serializable]
    public class BallPrefabTypeSettings 
    {
        public float damage;
        public float speed;
    }
}
