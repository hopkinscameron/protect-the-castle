using System.Collections.Generic;
using ProtectTheCastle.Environment.AISpawns;
using ProtectTheCastle.Environment.NavigationSpawns;
using ProtectTheCastle.Environment.TowerSpawns;
using ProtectTheCastle.Players;
using UnityEngine;

namespace ProtectTheCastle.Game
{
    public class GameSettingsManager : MonoBehaviour, IGameSettingsManager
    {
        public static GameSettingsManager Instance { get; private set; }
        
        public bool gameInProgress { get; private set; }
        public bool pickingPlayers { get; private set; }
        public bool pickingTowers { get; private set; }
        public bool isPlayer1Turn { get; private set; }
        public BallPrefabSettings ballPrefabSettings { get; private set; }
        public CastlePrefabSettings castlePrefabSettings { get; private set; }
        public PlayerPrefabSettings playerPrefabSettings { get; private set; }
        public TowerPrefabSettings towerPrefabSettings { get; private set; }

        private bool _playersPicked;
        private bool _towersPicked;
        private List<GameObject> player1;
        private List<GameObject> player2;

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

            // TODO: remove me
            StartGame();
            SpawnPlayers();
        }

        public bool StartGame()
        {
            if (!gameInProgress)
            {
                isPlayer1Turn = Random.Range(0, 2) == 0;
                gameInProgress = true;
            }


            return gameInProgress;
        }

        public bool StartPickingPlayers()
        {
            if (gameInProgress && !_playersPicked)
            {
                pickingPlayers = true;
                return true;
            }

            return false;
        }
        
        public bool StartPickingTowers()
        {
            if (gameInProgress && _playersPicked && !_towersPicked)
            {
                pickingPlayers = false;
                pickingTowers = true;
                return true;
            }

            return false;
        }

        private void SpawnPlayers()
        {
            player2 = new List<GameObject>();

            var player1Castle = GameObject.FindGameObjectWithTag("Player Castle");
            var player2Castle = GameObject.FindGameObjectWithTag("Enemy Castle");
            var player1HomeBaseSpawn = SpawnPlayerNavigationPoints.Instance.GetHomeBaseSpawn(player1Castle);
            var player2HomeBaseSpawn = SpawnPlayerNavigationPoints.Instance.GetHomeBaseSpawn(player2Castle);
            player1 = new List<GameObject> {
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Soldier", typeof(GameObject)),
                    new Vector3(player1HomeBaseSpawn.transform.position.x, 0, player1HomeBaseSpawn.transform.position.z),
                    transform.rotation),
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Hero", typeof(GameObject)),
                    new Vector3(player1HomeBaseSpawn.transform.position.x + 2f, 0, player1HomeBaseSpawn.transform.position.z),
                    transform.rotation),
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Dog", typeof(GameObject)),
                    new Vector3(player1HomeBaseSpawn.transform.position.x - 2f, 0, player1HomeBaseSpawn.transform.position.z),
                    transform.rotation)
            };

            var turnAroundAngle = transform.rotation;
            turnAroundAngle.eulerAngles = new Vector3(
                turnAroundAngle.eulerAngles.x,
                turnAroundAngle.eulerAngles.y + 180,
                turnAroundAngle.eulerAngles.z
            );
            player2 = new List<GameObject> {
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Golem", typeof(GameObject)),
                    new Vector3(player2HomeBaseSpawn.transform.position.x, 0, player2HomeBaseSpawn.transform.position.z),
                    turnAroundAngle),
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Grunt", typeof(GameObject)),
                    new Vector3(player2HomeBaseSpawn.transform.position.x + 2f, 0, player2HomeBaseSpawn.transform.position.z),
                    turnAroundAngle),
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Lich", typeof(GameObject)),
                    new Vector3(player2HomeBaseSpawn.transform.position.x - 2f, 0, player2HomeBaseSpawn.transform.position.z),
                    transform.rotation)
            };

            foreach(var player1GameObject in player1)
            {
                player1GameObject.GetComponent<Player>().SetHome(player1Castle);
            }

            foreach(var player2GameObject in player2)
            {
                player2GameObject.GetComponent<Player>().SetHome(player2Castle);
            }
        }
    }
}
