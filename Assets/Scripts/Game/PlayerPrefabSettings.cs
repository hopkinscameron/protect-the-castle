namespace ProtectTheCastle.Game
{
    [System.Serializable]
    public class PlayerPrefabSettings 
    {
        public PlayerPrefabTypeSettings normal;
        public PlayerPrefabTypeSettings fire;
        public PlayerPrefabTypeSettings grass;
        public PlayerPrefabTypeSettings water;
        public PlayerPrefabTypeSettings ice;
        public PlayerPrefabTypeSettings poison;
    }

    [System.Serializable]
    public class PlayerPrefabTypeSettings 
    {
        public float damage;
        public float health;
        public float speed;
    }
}
