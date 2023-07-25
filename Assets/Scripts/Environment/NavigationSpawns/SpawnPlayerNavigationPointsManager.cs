using System;
using System.Collections.Generic;
using System.Linq;
using ProtectTheCastle.Shared;
using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public class SpawnPlayerNavigationPoints : MonoBehaviour, ISpawnPlayerNavigationPointsManager
    {
        public static SpawnPlayerNavigationPoints Instance { get; private set; }

        private const float DEFAULT_Y_POSITION = 1;
        private IReadOnlyList<GameObject> _common;
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
            var spawnLevelDirectionSettings = JsonUtility.FromJson<PlayerNavigationSpawnLevel>(contents.text).forest;

            _common = SpawnNavigationPoints(spawnLevelDirectionSettings.common, "Common");
            _left = SpawnNavigationPoints(spawnLevelDirectionSettings.left, "Left");
            _straight = SpawnNavigationPoints(spawnLevelDirectionSettings.straight, "Straight");
            _right = SpawnNavigationPoints(spawnLevelDirectionSettings.right, "Right");
        }

        public GameObject GetHomeBaseSpawn(GameObject homeBase)
        {
            if (homeBase.tag.Equals(Constants.PLAYER_1_CASTLE_TAG, StringComparison.OrdinalIgnoreCase))
            {
                return _common[0];
            }

            return _common[_common.Count - 1];
        }

        public GameObject GetNextTarget(GameObject homeBase, Vector3 currentPosition)
        {
            var currentSpawnSystem = _common;
            var currentIndex = GetCurrentIndex(currentSpawnSystem, currentPosition);

            if (currentIndex == -1)
            {
                currentSpawnSystem = _straight;
                currentIndex = GetCurrentIndex(currentSpawnSystem, currentPosition);
            }

            if (currentIndex == -1)
            {
                currentSpawnSystem = _left;
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
            var currentSpawnSystem = _common;
            var currentIndex = GetCurrentIndex(currentSpawnSystem, currentTarget);
            if (currentIndex == -1)
            {
                currentSpawnSystem = _straight;
                currentIndex = GetCurrentIndex(currentSpawnSystem, currentTarget);
            }

            if (currentIndex == -1)
            {
                currentSpawnSystem = _left;
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

        private IReadOnlyList<GameObject> SpawnNavigationPoints(IReadOnlyList<PlayerNavigationSpawnCoordinates> spawns, string positionTitle)
        {
            return spawns.Select((spawn, i) => {
                GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                capsule.name = "Player Spawn Capsule " + positionTitle + " " + i;
                capsule.transform.position = new Vector3(spawn.x, DEFAULT_Y_POSITION, spawn.z);
                capsule.GetComponent<Renderer>().material.color = Color.white;
                var playerNavigationSpawn = capsule.AddComponent<PlayerNavigationSpawn>();
                playerNavigationSpawn.isDecisionSpawn = spawn.isDecisionSpawn;
                capsule.GetComponent<CapsuleCollider>().isTrigger = true;
                return capsule;
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
            var isDecisionSpawn = currentSpawnSystem[currentIndex].GetComponent<PlayerNavigationSpawn>().isDecisionSpawn;
            var nextIndex = -1;
            var isPlayer1 = homeBase.tag.Equals(Constants.PLAYER_1_CASTLE_TAG, StringComparison.OrdinalIgnoreCase);

            if (isDecisionSpawn)
            {
                // TODO: this is test for AI, instead we'd know the isDecisionSpawn and we'd know the next path
                var randomDirection = UnityEngine.Random.Range(0, 3);
                var path = _straight;
                switch (randomDirection)
                {
                    case 0:
                        path = _left;
                        break;
                    case 1:
                        path = _right;
                        break;
                }

                if (isPlayer1)
                {
                    return _straight[0];
                }
                
                return path[path.Count - 1];
            }

            if (isPlayer1)
            {
                nextIndex = currentIndex + 1;
                return nextIndex < currentSpawnSystem.Count ? currentSpawnSystem[nextIndex] : null;
            }

            nextIndex = currentIndex - 1;
            return nextIndex > -1 ? currentSpawnSystem[nextIndex] : null;
        }
    }
}
