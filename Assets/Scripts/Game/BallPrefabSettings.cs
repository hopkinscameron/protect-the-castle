namespace ProtectTheCastle.Game
{
    [System.Serializable]
    public class BallPrefabSettings 
    {
        public BallPrefabTypeSettings normal;
        public BallPrefabTypeSettings fire;
        public BallPrefabTypeSettings grass;
        public BallPrefabTypeSettings water;
        public BallPrefabTypeSettings ice;
        public BallPrefabTypeSettings poison;
    }

    [System.Serializable]
    public class BallPrefabTypeSettings 
    {
        public float damage;
        public float speed;
    }
}
