using System;
using System.Collections.Generic;
using System.Linq;
using ProtectTheCastle.Players.Enums;
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
            if (homeBase.tag.Equals(Constants.Player1.CASTLE_TAG, StringComparison.OrdinalIgnoreCase))
            {
                return _common[0];
            }

            return _common[_common.Count - 1];
        }

        public GameObject GetNextTarget(GameObject homeBase, Vector3 currentPosition, EnumPlayerMoveDirection directionToMove)
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

            return GetNextTargetBasedOnCastle(homeBase, currentSpawnSystem, currentIndex, directionToMove);
        }

        public GameObject GetNextTarget(GameObject homeBase, GameObject currentTarget, EnumPlayerMoveDirection directionToMove)
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

            return GetNextTargetBasedOnCastle(homeBase, currentSpawnSystem, currentIndex, directionToMove);
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
                playerNavigationSpawn.isPlayer1WinCondition = positionTitle == "Common" && i == spawns.Count - 1;
                playerNavigationSpawn.isPlayer2WinCondition = positionTitle == "Common" && i == 0;
                capsule.GetComponent<CapsuleCollider>().enabled = false;
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

        private GameObject GetNextTargetBasedOnCastle(GameObject homeBase, IReadOnlyList<GameObject> currentSpawnSystem, int currentIndex, EnumPlayerMoveDirection directionToMove)
        {
            var isPlayer1 = homeBase.tag.Equals(Constants.Player1.CASTLE_TAG, StringComparison.OrdinalIgnoreCase);
            var nextIndex = isPlayer1 ? currentIndex + 1 : currentIndex - 1;
            var nextSpawnSystem = currentSpawnSystem;

            if (nextSpawnSystem == _common)
            {
                
                var isDecisionSpawn = (nextSpawnSystem[currentIndex].GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn).isDecisionSpawn;

                if (isDecisionSpawn)
                {
                    if (!isPlayer1)
                    {
                        var potentialWinSpawn = nextIndex > -1 ? nextSpawnSystem[nextIndex] : null;
                        if ((potentialWinSpawn?.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn).isPlayer2WinCondition == true)
                        {
                            return potentialWinSpawn;
                        }

                        directionToMove = EnumPlayerMoveDirection.Left;
                        var randomDirection = UnityEngine.Random.Range(0, 3);
                        switch (randomDirection)
                        {
                            case 1:
                                directionToMove = EnumPlayerMoveDirection.Forward;
                                break;
                            case 2:
                                directionToMove = EnumPlayerMoveDirection.Right;
                                break;
                        }

                        // TODO: used for testing
                        directionToMove = EnumPlayerMoveDirection.Forward;
                    }
                    else
                    {
                        var potentialWinSpawn = nextIndex < nextSpawnSystem.Count ? nextSpawnSystem[nextIndex] : null;
                        if ((potentialWinSpawn?.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn).isPlayer1WinCondition == true)
                        {
                            return potentialWinSpawn;
                        }
                    }

                    if (directionToMove == EnumPlayerMoveDirection.Unknown)
                    {
                        return null;
                    }

                    if (directionToMove == EnumPlayerMoveDirection.Left)
                    {
                        nextSpawnSystem = _left;
                    }
                    else if (directionToMove == EnumPlayerMoveDirection.Forward)
                    {
                        nextSpawnSystem = _straight;
                    }
                    else if (directionToMove == EnumPlayerMoveDirection.Right)
                    {
                        nextSpawnSystem = _right;
                    }

                    nextIndex = isPlayer1 ? 0 : nextSpawnSystem.Count - 1;
                }
            }

            if (isPlayer1)
            {
                if (nextIndex == nextSpawnSystem.Count)
                {
                    nextSpawnSystem = _common;
                    for (var x = nextSpawnSystem.Count - 1; x > -1; x--)
                    {
                        if ((nextSpawnSystem[x]?.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn).isDecisionSpawn)
                        {
                            nextIndex = x;
                            break;
                        }
                    }
                }

                return nextSpawnSystem[nextIndex];
            }
            else if (!isPlayer1)
            {
                if (nextIndex == -1)
                {
                    nextSpawnSystem = _common;
                    for (var x = 0; x < nextSpawnSystem.Count; x++)
                    {
                        if ((nextSpawnSystem[x]?.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn).isDecisionSpawn)
                        {
                            nextIndex = x;
                            break;
                        }
                    }
                }

                return nextSpawnSystem[nextIndex];
            }

            return null;
        }
    }
}
