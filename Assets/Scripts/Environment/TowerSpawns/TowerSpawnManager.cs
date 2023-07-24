using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProtectTheCastle.Environment.TowerSpawns
{
    public class TowerSpawnManager : MonoBehaviour, ITowerSpawnManager
    {
        public static TowerSpawnManager Instance { get; private set; }

        private const float DEFAULT_Y_POSITION = 0.0001938343f;

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

        public GameObject Spawn(GameObject spawnObject)
        {
            return Instantiate(spawnObject, new Vector3(spawnObject.transform.position.x, DEFAULT_Y_POSITION, spawnObject.transform.position.z), transform.rotation);
        }

        private IReadOnlyList<GameObject> SpawnTowerPoints(IReadOnlyList<TowerSpawn> spawns)
        {
            return spawns.Select(spawn => {
                GameObject cube = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(spawn.x, DEFAULT_Y_POSITION, spawn.z), transform.rotation);
                cube.GetComponent<Renderer>().material.color = Color.blue;
                return cube;
            }).ToList();
        }
    }
}
