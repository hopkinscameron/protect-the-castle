using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public class SpawnPlayerNavigationPoints : MonoBehaviour, ISpawnPlayerNavigationPointsManager
    {
        public static SpawnPlayerNavigationPoints Instance { get; private set; }

        private const float DEFAULT_Y_POSITION = 0;
        private IReadOnlyList<GameObject> _straight;
        private IReadOnlyList<GameObject> _left;
        private IReadOnlyList<GameObject> _right;

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
            var contents = (TextAsset) Resources.Load("Settings/player-navigation-path-spawns", typeof(TextAsset));
            var spawnSettings = JsonUtility.FromJson<PlayerNavigationSpawnSettings>(contents.text);

            _left = SpawnNavigationPoints(spawnSettings.left);
            _straight = SpawnNavigationPoints(spawnSettings.straight);
            _right = SpawnNavigationPoints(spawnSettings.right);
        }

        public GameObject GetHomeBaseSpawn(GameObject homeBase)
        {
            if (homeBase.tag.Equals("Player Castle", StringComparison.OrdinalIgnoreCase))
            {
                return _straight[0];
            }

            return _straight[_straight.Count - 1];
        }

        public GameObject GetNextTarget(GameObject homeBase, Vector3 currentPosition)
        {
            var currentSpawnSystem = _left;
            var currentIndex = GetCurrentIndex(currentSpawnSystem, currentPosition);
            if (currentIndex == -1)
            {
                currentSpawnSystem = _straight;
                currentIndex = GetCurrentIndex(currentSpawnSystem, currentPosition);
            }

            if (currentIndex == -1)
            {
                currentSpawnSystem = _right;
                currentIndex = GetCurrentIndex(currentSpawnSystem, currentPosition);
            }

            if (currentIndex == -1)
            {
                return null;
            }

            return GetNextTargetBasedOnCastle(homeBase, currentSpawnSystem, currentIndex);
        }

        public GameObject GetNextTarget(GameObject homeBase, GameObject currentTarget)
        {
            var currentSpawnSystem = _left;
            var currentIndex = GetCurrentIndex(currentSpawnSystem, currentTarget);
            if (currentIndex == -1)
            {
                currentSpawnSystem = _straight;
                currentIndex = GetCurrentIndex(currentSpawnSystem, currentTarget);
            }

            if (currentIndex == -1)
            {
                currentSpawnSystem = _right;
                currentIndex = GetCurrentIndex(currentSpawnSystem, currentTarget);
            }

            if (currentIndex == -1)
            {
                return null;
            }

            return GetNextTargetBasedOnCastle(homeBase, currentSpawnSystem, currentIndex);
        }

        private IReadOnlyList<GameObject> SpawnNavigationPoints(IReadOnlyList<PlayerNavigationSpawn> spawns)
        {
            return spawns.Select(spawn => {
                GameObject cube = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(spawn.x, DEFAULT_Y_POSITION, spawn.z), transform.rotation);
                cube.GetComponent<Renderer>().material.color = Color.white;
                return cube;
            }).ToList();
        }

        private int GetCurrentIndex(IReadOnlyList<GameObject> spawns, Vector3 currentPosition)
        {
            for (var x = 0; x < spawns.Count; x++)
            {
                var spawn = spawns[x];
                var dist = Vector3.Distance(currentPosition, new Vector3(spawn.transform.position.x, DEFAULT_Y_POSITION, spawn.transform.position.z));
                if (dist < 5)
                {
                    return x;
                }
            }

            return -1;
        }

        private int GetCurrentIndex(IReadOnlyList<GameObject> spawns, GameObject currentTarget)
        {
            for (var x = 0; x < spawns.Count; x++)
            {
                if (spawns[x] == currentTarget)
                {
                    return x;
                }
            }

            return -1;
        }

        private GameObject GetNextTargetBasedOnCastle(GameObject homeBase, IReadOnlyList<GameObject> currentSpawnSystem, int currentIndex)
        {
            var nextIndex = -1;
            if (homeBase.tag.Equals("Player Castle", StringComparison.OrdinalIgnoreCase))
            {
                nextIndex = currentIndex + 1;
                return nextIndex < currentSpawnSystem.Count ? currentSpawnSystem[nextIndex] : null;
            }

            nextIndex = currentIndex - 1;
            return nextIndex > -1 ? currentSpawnSystem[nextIndex] : null;
        }
    }
}