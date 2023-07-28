using ProtectTheCastle.Environment.AISpawns;
using ProtectTheCastle.Environment.NavigationSpawns;
using ProtectTheCastle.Environment.TowerSpawns;
using UnityEngine;

namespace ProtectTheCastle.Game
{
    public class GameSettingsManager : MonoBehaviour, IGameSettingsManager
    {
        public static GameSettingsManager Instance { get; private set; }

        public BallPrefabSettings ballPrefabSettings { get; private set; }
        public CastlePrefabSettings castlePrefabSettings { get; private set; }
        public PlayerPrefabSettings playerPrefabSettings { get; private set; }
        public TowerPrefabSettings towerPrefabSettings { get; private set; }
        public bool loaded { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            }
        }

        private void Start()
        {
            SpawnPlayerNavigationPoints.Instance.Load();
            TowerSpawnManager.Instance.Load();
            AISpawnManager.Instance.Load();
            AISpawnManager.Instance.SpawnSome();

            var contents = (TextAsset) Resources.Load("Settings/ball", typeof(TextAsset));
            ballPrefabSettings = JsonUtility.FromJson<BallPrefabSettings>(contents.text);

            contents = (TextAsset) Resources.Load("Settings/castle", typeof(TextAsset));
            castlePrefabSettings = JsonUtility.FromJson<CastlePrefabSettings>(contents.text);

            contents = (TextAsset) Resources.Load("Settings/player", typeof(TextAsset));
            playerPrefabSettings = JsonUtility.FromJson<PlayerPrefabSettings>(contents.text);

            contents = (TextAsset) Resources.Load("Settings/tower", typeof(TextAsset));
            towerPrefabSettings = JsonUtility.FromJson<TowerPrefabSettings>(contents.text);
        }
    }
}
