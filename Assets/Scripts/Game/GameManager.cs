using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ProtectTheCastle.Environment.NavigationSpawns;
using ProtectTheCastle.Players;
using ProtectTheCastle.Players.Enums;
using ProtectTheCastle.Shared;
using UnityEngine;

namespace ProtectTheCastle.Game
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public static GameManager Instance { get; private set; }

        public bool gameInProgress { get; private set; }
        public bool pickingPlayers { get; private set; }
        public bool pickingTowers { get; private set; }
        public bool isPlayer1Turn { get; private set; }

        private bool _playersPicked;
        private bool _towersPicked;
        private bool _gameReady;
        private IReadOnlyList<GameObject> _player1;
        private IReadOnlyList<GameObject> _player2;
        private GameObject _camera;

        // TODO: remove me, used for testing
        private GameObject _player2LastCharacterMoved;
        private int attemptsToMove = 0;
        private bool moving = false;
        private EnumPlayerMoveDirection directionClicked;

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

            _camera = GameObject.FindGameObjectWithTag(Constants.VIRTUAL_CAMERA_TAG);
        }

        private void Update()
        {
            directionClicked = GetPlayerMoveDirectionBasedOnInput();
            if(directionClicked != EnumPlayerMoveDirection.Unknown && isPlayer1Turn && !moving)
            {
                moving = true;
                AttemptToMovePlayer();
            }
        }

        public bool StartGame()
        {
            if (!gameInProgress)
            {
                // Debug.Log("Game Started");
                isPlayer1Turn = true; // UnityEngine.Random.Range(0, 2) == 0;
                gameInProgress = true;
                StartPickingPlayers();

                // TODO: remove me, used for testing
                FinishPickingPlayers();
                StartPickingTowers();
                FinishPickingTowers();
                SpawnPlayers();

                FocusCharacter(_player1[0]);
            }

            return gameInProgress;
        }

        public bool StartPickingPlayers()
        {
            if (gameInProgress && !_playersPicked)
            {
                // Debug.Log("Picking Players");
                pickingPlayers = true;
                return true;
            }

            return false;
        }

        public bool FinishPickingPlayers()
        {
            if (gameInProgress && pickingPlayers && !_playersPicked)
            {
                // Debug.Log("Finished Picking Players");
                pickingPlayers = false;
                _playersPicked = true;
                return true;
            }

            return false;
        }
        
        public bool StartPickingTowers()
        {
            if (gameInProgress && !pickingPlayers && _playersPicked && !_towersPicked)
            {
                // Debug.Log("Picking Towers");
                pickingTowers = true;
                return true;
            }

            return false;
        }

        public bool FinishPickingTowers()
        {
            if (gameInProgress && pickingTowers && !_towersPicked)
            {
                // Debug.Log("Finished Picking Towers");
                pickingTowers = false;
                _towersPicked = true;
                return true;
            }

            return false;
        }

        public void MovePlayer(GameObject characterToMove)
        {
            if (!gameInProgress || !_gameReady || characterToMove == null
                || (!isPlayer1Turn && attemptsToMove >= (_player2.Count * 2))) return;

            if ((isPlayer1Turn && characterToMove.tag.Equals(Constants.Player1.TAG, StringComparison.OrdinalIgnoreCase))
                || (!isPlayer1Turn && characterToMove.tag.Equals(Constants.Player2.TAG, StringComparison.OrdinalIgnoreCase)))
            {
                if (attemptsToMove == 0)
                {
                    Debug.Log(isPlayer1Turn ? "Player 1 turn started" : "Player 2 turn started");
                }

                if (!isPlayer1Turn)
                {
                    _player2LastCharacterMoved = characterToMove;
                }

                var couldMove = (characterToMove.GetComponent(typeof(IPlayer)) as IPlayer).Move(directionClicked);
                if (!couldMove && !isPlayer1Turn)
                {
                    attemptsToMove++;
                    AttemptToMovePlayer();
                    return;
                }

                attemptsToMove = 0;
            }
        }

        public void EndTurn()
        {
            Debug.Log(isPlayer1Turn ? "Player 1 turn ended" : "Player 2 turn ended");
            isPlayer1Turn = !isPlayer1Turn;
            FocusCharacter(isPlayer1Turn ? _player1[0] : _player2[0]);
            
            if (!isPlayer1Turn)
            {
                
                AttemptToMovePlayer();
            }
            else
            {
                moving = false;
            }
        }

        public bool EndGame(GameObject winner)
        {
            if (gameInProgress)
            {
                gameInProgress = false;
                Debug.Log("The winner is " + winner.tag);
            }

            return gameInProgress;
        }

        private void SpawnPlayers()
        {
            _player2 = new List<GameObject>();

            var player1ForwardAngle = transform.rotation;
            player1ForwardAngle.eulerAngles = new Vector3(0, 0, 0);

            var player1Castle = GameObject.FindGameObjectWithTag(Constants.Player1.CASTLE_TAG);
            var player2Castle = GameObject.FindGameObjectWithTag(Constants.Player2.CASTLE_TAG);
            var player1HomeBaseSpawn = SpawnPlayerNavigationPoints.Instance.GetHomeBaseSpawn(player1Castle);
            var player2HomeBaseSpawn = SpawnPlayerNavigationPoints.Instance.GetHomeBaseSpawn(player2Castle);
            _player1 = new List<GameObject> {
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Soldier", typeof(GameObject)),
                    // TODO: used for testing:
                    // new Vector3(50, 0, 81),
                    new Vector3(player1HomeBaseSpawn.transform.position.x, 0, player1HomeBaseSpawn.transform.position.z),
                    player1ForwardAngle),
                // Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Hero", typeof(GameObject)),
                //     new Vector3(player1HomeBaseSpawn.transform.position.x + 2f, 0, player1HomeBaseSpawn.transform.position.z),
                //     player1ForwardAngle),
                // Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Dog", typeof(GameObject)),
                //     new Vector3(player1HomeBaseSpawn.transform.position.x - 2f, 0, player1HomeBaseSpawn.transform.position.z),
                //     player1ForwardAngle)
            };

            var player2ForwardAngle = transform.rotation;
            player2ForwardAngle.eulerAngles = new Vector3(0, 180, 0);
            _player2 = new List<GameObject> {
                // Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Golem", typeof(GameObject)),
                //     new Vector3(player2HomeBaseSpawn.transform.position.x, 0, player2HomeBaseSpawn.transform.position.z),
                //     player2ForwardAngle),
                // Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Grunt", typeof(GameObject)),
                //     new Vector3(player2HomeBaseSpawn.transform.position.x + 2f, 0, player2HomeBaseSpawn.transform.position.z),
                //     player2ForwardAngle),
                Instantiate((GameObject) Resources.Load("Prefabs/Soldiers/Lich", typeof(GameObject)),
                    new Vector3(player2HomeBaseSpawn.transform.position.x - 2f, 0, player2HomeBaseSpawn.transform.position.z),
                    player2ForwardAngle)
            };

            foreach(var player1GameObject in _player1)
            {
                (player1GameObject.GetComponent(typeof(IPlayer)) as IPlayer).SetHome(player1Castle);
                player1GameObject.tag = Constants.Player1.TAG;
            }

            foreach(var player2GameObject in _player2)
            {
                (player2GameObject.GetComponent(typeof(IPlayer)) as IPlayer).SetHome(player2Castle);
                player2GameObject.tag = Constants.Player2.TAG;
            }

            // Debug.Log("Players Spawned, Game Ready");
            _gameReady = true;
        }

        private void AttemptToMovePlayer()
        {
            GameObject characterToMove = null;
            if (isPlayer1Turn)
            {
                characterToMove = _player1[0];
            }
            else
            {
                for (var x = 0; x < _player2.Count; x++)
                {
                    if (_player2[x] == _player2LastCharacterMoved)
                    {
                        characterToMove = _player2[(x + 1) % _player2.Count];
                        break;
                    }
                }

                if (characterToMove == null)
                {
                    characterToMove = _player2[UnityEngine.Random.Range(0, _player2.Count)];
                }
            }

            MovePlayer(characterToMove);
        }

        private void FocusCharacter(GameObject characterToFocus)
        {
            _camera.GetComponent<CinemachineVirtualCamera>().Follow = characterToFocus.transform;
            _camera.GetComponent<CinemachineVirtualCamera>().LookAt = characterToFocus.transform;
        }

        private EnumPlayerMoveDirection GetPlayerMoveDirectionBasedOnInput()
        {
            if (Input.GetKeyUp("left"))
            {
                // Debug.Log("Selected to move left");
                return EnumPlayerMoveDirection.Left;
            }
            else if (Input.GetKeyUp("right"))
            {
                // Debug.Log("Selected to move right");
                return EnumPlayerMoveDirection.Right;
            }
            else if (Input.GetKeyUp("up"))
            {
                // Debug.Log("Selected to move forward");
                return EnumPlayerMoveDirection.Forward;
            }

            return EnumPlayerMoveDirection.Unknown;
        }
    }
}
