using System.Collections.Generic;
using System.Linq;
using ProtectTheCastle.Shared;
using UnityEngine;

namespace ProtectTheCastle.Environment.TowerSpawns
{
    public class TowerSpawnManager : MonoBehaviour, ITowerSpawnManager
    {
        public static TowerSpawnManager Instance { get; private set; }

        private const float DEFAULT_Y_POSITION = 0f;

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

        public void Load()
        {
            var contents = (TextAsset) Resources.Load("Settings/tower-spawns");
            var spawnSettings = JsonUtility.FromJson<TowerSpawnSettings>(contents.text);
            SpawnTowerPoints(spawnSettings.forest);
        }

        public GameObject Spawn(GameObject spawnObject, TowerSpawn spawnInfo, bool isPlayer)
        {
            var towerAngle = transform.rotation;
            towerAngle.eulerAngles = new Vector3(0, spawnInfo.rotation, 0);
            spawnObject.tag = isPlayer ? Constants.PLAYER_1_TOWER_TAG : Constants.PLAYER_2_TOWER_TAG;
            return Instantiate(spawnObject, new Vector3(spawnInfo.x, DEFAULT_Y_POSITION, spawnInfo.z), towerAngle);
        }

        private IReadOnlyList<GameObject> SpawnTowerPoints(IReadOnlyList<TowerSpawn> spawns)
        {
            return spawns.Select((spawn, i) => {
                var isPlayer1 = i % 2 == 0;
                var towerType = isPlayer1 ? "ForestTowerCanon_Blue" : "ForestTowerCanon_Red";
                return Spawn((GameObject) Resources.Load("Prefabs/Towers/" + towerType), spawn, isPlayer1);
            }).ToList();
        }
    }
}
