using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProtectTheCastle.Environment.AISpawns
{
    public class AISpawnManager : MonoBehaviour, IAISpawnManager
    {
        public static AISpawnManager Instance { get; private set; }

        private const float DEFAULT_Y_POSITION = 0;
        private const float MAX_RANDOMNESS = 100;
        private IReadOnlyList<GameObject> _aiPrefabs;
        private IReadOnlyList<GameObject> _spawns;

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
            var contents = (TextAsset) Resources.Load("Settings/ai-spawns", typeof(TextAsset));
            var spawnSettings = JsonUtility.FromJson<AISpawnSettings>(contents.text);
            _spawns = SpawnAIPoints(spawnSettings.forest);

            var prefabs = Resources.LoadAll("Prefabs/AI", typeof(GameObject));
            _aiPrefabs = prefabs.Select(prefab => (GameObject) prefab).ToList();
        }

        public IReadOnlyList<GameObject> SpawnSome()
        {
            var ai = new List<GameObject>();
            var numToSpawn = Random.Range(5, 10);
            for (var x = 0; x < numToSpawn; x++)
            {
                var prefabIndex = Random.Range(0, _aiPrefabs.Count);
                var spawnPointIndex = Random.Range(0, _spawns.Count);
                var spawnPoint = _spawns[spawnPointIndex];
                ai.Add(Instantiate(_aiPrefabs[prefabIndex], new Vector3(spawnPoint.transform.position.x, DEFAULT_Y_POSITION, spawnPoint.transform.position.z), transform.rotation));
            }

            return ai;
        }

        public GameObject GetNextTarget(Vector3 currentPosition)
        {
            var currentIndex = GetCurrentIndex(currentPosition);
            if (currentIndex == -1)
            {
                return null;
            }

            return GetNextTarget(currentIndex);
        }

        public GameObject GetNextTarget(GameObject currentTarget)
        {
            var currentIndex = GetCurrentIndex(currentTarget);
            if (currentIndex == -1)
            {
                return null;
            }

            return GetNextTarget(currentIndex);
        }

        private IReadOnlyList<GameObject> SpawnAIPoints(IReadOnlyList<AISpawn> spawns)
        {
            return spawns.Select(spawn => {
                GameObject cube = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(spawn.x, DEFAULT_Y_POSITION, spawn.z), transform.rotation);
                cube.GetComponent<Renderer>().material.color = Color.cyan;
                return cube;
            }).ToList();
        }

        private int GetCurrentIndex(Vector3 currentPosition)
        {
            for (var x = 0; x < _spawns.Count; x++)
            {
                var spawn = _spawns[x];
                var dist = Vector3.Distance(currentPosition, new Vector3(spawn.transform.position.x, DEFAULT_Y_POSITION, spawn.transform.position.z));
                if (dist < 15)
                {
                    return x;
                }
            }

            return -1;
        }

        private int GetCurrentIndex(GameObject currentTarget)
        {
            for (var x = 0; x < _spawns.Count; x++)
            {
                if (_spawns[x] == currentTarget)
                {
                    return x;
                }
            }

            return -1;
        }

        private GameObject GetNextTarget(int currentIndex)
        {
            var maxRandomness = 100;
            var spawnPointIndex = Random.Range(0, _spawns.Count);
            while (spawnPointIndex == currentIndex || maxRandomness < MAX_RANDOMNESS)
            {
                spawnPointIndex = Random.Range(0, _spawns.Count);
                maxRandomness++;
            }

            if (spawnPointIndex == currentIndex)
            {
                spawnPointIndex = 0;
            }

            return _spawns[spawnPointIndex];
        }
    }
}
