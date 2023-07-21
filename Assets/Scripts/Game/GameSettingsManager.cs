using UnityEngine;

namespace ProtectTheCastle.Game
{
    public class GameSettingsManager : MonoBehaviour
    {
        public static GameSettingsManager Instance { get; private set; }
        public BallPrefabSettings ballPrefabSettings { get; private set; }
        public CastlePrefabSettings castlePrefabSettings { get; private set; }
        public PlayerPrefabSettings playerPrefabSettings { get; private set; }
        public TowerPrefabSettings towerPrefabSettings { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            }

            TextAsset contents = (TextAsset) Resources.Load("Settings/ball");
            ballPrefabSettings = JsonUtility.FromJson<BallPrefabSettings>(contents.text);

            contents = (TextAsset) Resources.Load("Settings/castle");
            castlePrefabSettings = JsonUtility.FromJson<CastlePrefabSettings>(contents.text);

            contents = (TextAsset) Resources.Load("Settings/player");
            playerPrefabSettings = JsonUtility.FromJson<PlayerPrefabSettings>(contents.text);

            contents = (TextAsset) Resources.Load("Settings/tower");
            towerPrefabSettings = JsonUtility.FromJson<TowerPrefabSettings>(contents.text);
        }
    }
}
