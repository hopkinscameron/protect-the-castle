namespace ProtectTheCastle.Game
{
    [System.Serializable]
    public class TowerPrefabSettings 
    {
        public TowerPrefabTypeSettings normal;
        public TowerPrefabTypeSettings fire;
        public TowerPrefabTypeSettings grass;
        public TowerPrefabTypeSettings water;
        public TowerPrefabTypeSettings ice;
        public TowerPrefabTypeSettings poison;
    }

    [System.Serializable]
    public class TowerPrefabTypeSettings 
    {
        public float coolDown;
        public float health;
        public float healthDecreaseAmount;
        public float minEngageDistance;
    }
}
