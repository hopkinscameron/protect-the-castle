using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ProtectTheCastle.Environment.NavigationSpawns;
using ProtectTheCastle.Players;
using ProtectTheCastle.Players.Enums;
using ProtectTheCastle.Shared;
using ProtectTheCastle.UI;
using UnityEngine;

namespace ProtectTheCastle.Game
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public static GameManager Instance { get; private set; }

        public bool gameStarted { get; private set; }
        public bool gameInProgress { get; private set; }
        public bool pickingPlayers { get; private set; }
        public bool pickingTowers { get; private set; }
        public bool isPlayer1Turn { get; private set; }

        private bool _playersPicked;
        private bool _towersPicked;
        private bool _gameReady;
        private List<GameObject> _player1;
        private List<GameObject> _player2;
        private CinemachineVirtualCamera _camera;

        // TODO: remove me, used for testing
        private GameObject _player2LastCharacterMoved;
        private int attemptsToMove = 0;
        private bool moving = false;

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

            _camera = GameObject.FindGameObjectWithTag(Constants.VIRTUAL_CAMERA_TAG).GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            if (!gameStarted) return;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                PauseOrResumeGame();
            }

            if (!gameInProgress) return;

            DirectionClicked(GetPlayerMoveDirectionBasedOnInput());
        }

        public bool StartGame()
        {
            if (!gameStarted)
            {
                isPlayer1Turn = true; // UnityEngine.Random.Range(0, 2) == 0;
                gameStarted = true;
                PauseOrResumeGame();
                StartPickingPlayers();

                // TODO: remove me, used for testing
                FinishPickingPlayers();
                StartPickingTowers();
                FinishPickingTowers();
                SpawnPlayers();

                FocusCharacter(_player1[0]);
                UIManager.Instance.ShowPlayerDirectionControls(true);
            }

            return gameStarted;
        }

        public bool PauseOrResumeGame()
        {
            gameInProgress = !gameInProgress;
            UIManager.Instance.PauseGame(!gameInProgress);
            Time.timeScale = !gameInProgress ? 0 : 1;

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

        public bool FinishPickingPlayers()
        {
            if (gameInProgress && pickingPlayers && !_playersPicked)
            {
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
                pickingTowers = true;
                return true;
            }

            return false;
        }

        public bool FinishPickingTowers()
        {
            if (gameInProgress && pickingTowers && !_towersPicked)
            {
                pickingTowers = false;
                _towersPicked = true;
                return true;
            }

            return false;
        }

        public void DirectionClicked(EnumPlayerMoveDirection direction)
        {
            if(direction != EnumPlayerMoveDirection.Unknown && isPlayer1Turn && !moving)
            {
                moving = true;
                GetCharacterAndMove(direction);
            }
        }

        public void AttackClicked()
        {
            UIManager.Instance.ShowPlayerBattleControls(false);
            var characterToMove = GetCharacter();
            var playerScript = characterToMove.GetComponent(typeof(IPlayer)) as IPlayer;
            BattlePlayer(playerScript);
        }

        public void PlayerDied(GameObject player)
        {
            if (player.tag.Equals(Constants.Player1.TAG))
            {
                _player1.Remove(player);
            }
            else if (player.tag.Equals(Constants.Player2.TAG))
            {
                _player2.Remove(player);
            }
            
            if (_player1.Count == 0)
            {
                EndGame(_player2[0]);
            }
            else if (_player2.Count == 0)
            {
                EndGame(_player1[0]);
            }
        }

        public void EndTurn()
        {
            if (_player1.Count == 0 || _player2.Count == 0) return;
            StartCoroutine("PauseBeforeSwitchingPlayers");
        }

        public bool EndGame(GameObject winner)
        {
            if (gameInProgress)
            {
                gameStarted = false;
                gameInProgress = false;
                winner.GetComponent<Animator>().SetTrigger(Constants.Animations.VICTORY_NAME);
                UIManager.Instance.ShowWinner(winner.tag.Equals(Constants.Player1.TAG, StringComparison.OrdinalIgnoreCase) ? "Player 1" : "Player 2");
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

            _gameReady = true;
        }

        private IEnumerator PauseBeforeSwitchingPlayers()
        {
            yield return new WaitForSeconds(2);
            isPlayer1Turn = !isPlayer1Turn;

            var characterToMove = GetCharacter();
            FocusCharacter(characterToMove);
            var playerScript = characterToMove.GetComponent(typeof(IPlayer)) as IPlayer;
            if (playerScript.inBattle)
            {
                if (!isPlayer1Turn)
                {
                    yield return new WaitForSeconds(2);
                    BattlePlayer(playerScript);
                }
                else
                {
                    UIManager.Instance.ShowPlayerBattleControls(true);
                }
            }
            else if (!isPlayer1Turn)
            {
                yield return new WaitForSeconds(2);
                GetCharacterAndMove(EnumPlayerMoveDirection.Forward);
            }
            else
            {
                UIManager.Instance.ShowPlayerDirectionControls(true);
                moving = false;
            }
        }

        private void FocusCharacter(GameObject characterToFocus)
        {
            _camera.Follow = characterToFocus.transform;
            _camera.LookAt = characterToFocus.transform;
        }

        private void GetCharacterAndMove(EnumPlayerMoveDirection directionClicked)
        {
            MovePlayer(GetCharacter(), directionClicked);
        }

        private GameObject GetCharacter()
        {
            if (isPlayer1Turn)
            {
                return _player1[0];
            }
            
            GameObject characterToMove = null;

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

            return characterToMove;
        }

        private void MovePlayer(GameObject characterToMove, EnumPlayerMoveDirection directionClicked)
        {
            if (!gameInProgress || !_gameReady || characterToMove == null
                || (!isPlayer1Turn && attemptsToMove >= (_player2.Count * 2))) return;

            if ((isPlayer1Turn && characterToMove.tag.Equals(Constants.Player1.TAG, StringComparison.OrdinalIgnoreCase))
                || (!isPlayer1Turn && characterToMove.tag.Equals(Constants.Player2.TAG, StringComparison.OrdinalIgnoreCase)))
            {
                if (!isPlayer1Turn)
                {
                    _player2LastCharacterMoved = characterToMove;
                }

                var playerScript = characterToMove.GetComponent(typeof(IPlayer)) as IPlayer;
                var couldMove = playerScript.Move(directionClicked);
                if (!couldMove && !isPlayer1Turn)
                {
                    attemptsToMove++;
                    GetCharacterAndMove(directionClicked);
                    return;
                }
                else if (couldMove && isPlayer1Turn)
                {
                    UIManager.Instance.ShowPlayerDirectionControls(false);
                }

                attemptsToMove = 0;
            }
        }

        private void BattlePlayer(IPlayer playerScript)
        {
            if (playerScript.inBattle)
            {
                playerScript.Attack();
            }
        }

        private EnumPlayerMoveDirection GetPlayerMoveDirectionBasedOnInput()
        {
            if (Input.GetKeyUp("left"))
            {
                return EnumPlayerMoveDirection.Left;
            }
            else if (Input.GetKeyUp("right"))
            {
                return EnumPlayerMoveDirection.Right;
            }
            else if (Input.GetKeyUp("up"))
            {
                return EnumPlayerMoveDirection.Forward;
            }

            return EnumPlayerMoveDirection.Unknown;
        }
    }
}
