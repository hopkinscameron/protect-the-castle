using System;
using System.Collections;
using System.Collections.Generic;
using ProtectTheCastle.Environment.NavigationSpawns;
using ProtectTheCastle.Game;
using ProtectTheCastle.Players.Enums;
using ProtectTheCastle.Shared;
using ProtectTheCastle.UI;
using UnityEngine;
using UnityEngine.AI;

namespace ProtectTheCastle.Players
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : MonoBehaviour, IPlayer
    {
        public float damage { get; private set; }
        public float health { get; private set; }
        public float speed { get; private set; }
        public bool alive { get; private set; } = true;
        public bool moving { get; private set; }
        public bool inBattle { get; private set; }

        [SerializeField]
        private EnumPlayerType _type;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private GameObject _homeCastle;
        private GameObject _lastTarget;
        private GameObject _nextTarget;
        private IPlayerNavigationSpawn _nextTargetNavSpawn;
        private INavMeshAgentHelper _navMeshAgentHelper;
        private bool _player1;
        private List<GameObject> _battleOpponents = new List<GameObject>();
        private IHealthBar _healthBar;
        private float _maxHealth;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _navMeshAgentHelper = new NavMeshAgentHelper();
            _healthBar = GetComponentInChildren(typeof(IHealthBar)) as IHealthBar;
        }

        private void Start()
        {
            PlayerPrefabTypeSettings settings = GetSettings();
            damage = settings.damage;
            health = settings.health;
            speed = settings.speed;
            _navMeshAgent.speed = speed;
            _maxHealth = settings.health;
            _healthBar.SetHealth(_maxHealth, health);
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.gameInProgress || !alive) return;

            if (moving && _nextTarget)
            {
                MovePlayer();
            }
        }

        public void BeginBattle()
        {
            inBattle = true;
            var _currentTargetNavSpawn = _lastTarget.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn;
            foreach (var potentialTarget in _currentTargetNavSpawn.occupiedBy)
            {
                if (_player1 && potentialTarget.tag.Equals(Constants.Player2.TAG))
                {
                    _battleOpponents.Add(potentialTarget);
                }
                else if (!_player1 && potentialTarget.tag.Equals(Constants.Player1.TAG))
                {
                    _battleOpponents.Add(potentialTarget);
                }
            }
        }

        public void Attack()
        {
            if (_battleOpponents.Count == 0) return;
            StartCoroutine("AttackOpponent");
        }

        public void Attacked(float amount)
        {
            health = health - amount;
            _healthBar.SetHealth(_maxHealth, health);

            if (health <= 0)
            {
                HandleDeath();
            }
            else
            {
                _animator.SetTrigger(Constants.Animations.GET_HIT_NAME);
            }
        }

        public void HandleDeath()
        {
            alive = false;
            moving = false;
            _navMeshAgent.isStopped = true;
            _animator.SetTrigger(Constants.Animations.DIE_NAME);
            StartCoroutine("Die");
        }

        public bool Move(EnumPlayerMoveDirection direction)
        {
            if (!alive || inBattle) return false;

            var previousTarget = _nextTarget;
            _nextTarget = _nextTarget == null
                ? SpawnPlayerNavigationPoints.Instance.GetNextTarget(_homeCastle, transform.position, direction)
                : SpawnPlayerNavigationPoints.Instance.GetNextTarget(_homeCastle, _nextTarget, direction);

            if (_nextTarget == null)
            {
                return false;
            }

            // Debug.Log(gameObject.name + " is moving towards " + _nextTarget.name + " at " + _nextTarget.transform.position.x + "," + _nextTarget.transform.position.z);
            _nextTargetNavSpawn = _nextTarget.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn;
            moving = _navMeshAgent.SetDestination(_nextTarget.transform.position);
            return moving;
        }

        public void SetHome(GameObject homeBase)
        {
            _player1 = homeBase.tag.Equals(Constants.Player1.CASTLE_TAG, StringComparison.OrdinalIgnoreCase);
            _homeCastle = homeBase;
        }

        private PlayerPrefabTypeSettings GetSettings()
        {
            switch (_type)
            {
                case EnumPlayerType.Hero:
                    return GameSettingsManager.Instance.playerPrefabSettings.hero;
                case EnumPlayerType.Soldier:
                    return GameSettingsManager.Instance.playerPrefabSettings.soldier;
                case EnumPlayerType.Mercenary:
                    return GameSettingsManager.Instance.playerPrefabSettings.mercenary;
                case EnumPlayerType.Warrior:
                    return GameSettingsManager.Instance.playerPrefabSettings.warrior;
                case EnumPlayerType.Magic:
                    return GameSettingsManager.Instance.playerPrefabSettings.magic;
                default:
                    return GameSettingsManager.Instance.playerPrefabSettings.normal;
            }
        }

        private void MovePlayer()
        {
            _animator.SetFloat(Constants.Animations.SPEED_NAME, _navMeshAgent.velocity.magnitude);
            moving = !_navMeshAgentHelper.ReachedDestination(_navMeshAgent);

            if (!moving && ((GameManager.Instance.isPlayer1Turn && _player1) || (!GameManager.Instance.isPlayer1Turn && !_player1)))
            {
                _lastTarget = _nextTarget;
                _nextTarget = null;

                var pns = _lastTarget.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn;
                if ((_player1 && pns.isPlayer1WinCondition) || (!_player1 && pns.isPlayer2WinCondition))
                {
                    GameManager.Instance.EndGame(gameObject);
                    _animator.SetTrigger(Constants.Animations.VICTORY_NAME);
                    return;
                }

                if (_nextTargetNavSpawn?.occupiedBy?.Count > 0)
                {
                    var hasEnemy = false;
                    foreach (var playerAtNavPoint in _nextTargetNavSpawn.occupiedBy)
                    {
                        if (playerAtNavPoint == gameObject) continue;

                        var playerScript = playerAtNavPoint.GetComponent(typeof(IPlayer)) as IPlayer;
                        if ((_player1 && playerAtNavPoint.tag.Equals(Constants.Player2.TAG))
                            || (!_player1 && playerAtNavPoint.tag.Equals(Constants.Player1.TAG)))
                        {
                            hasEnemy = true;
                            playerScript.BeginBattle();
                        }
                    }

                    if (hasEnemy)
                    {
                        BeginBattle();
                    }
                }

                _nextTargetNavSpawn = null;
                GameManager.Instance.EndTurn();
            }
        }

        private IEnumerator AttackOpponent()
        {
            var opponent = _battleOpponents[0];
            var playerScript = opponent.GetComponent(typeof(IPlayer)) as IPlayer;
            _animator.SetTrigger(Constants.Animations.ATTACK_NAME);

            yield return new WaitForSeconds(0.5f);

            playerScript.Attacked(damage);
            
            if (!playerScript.alive)
            {
                _battleOpponents.Remove(opponent);

                if (_battleOpponents.Count == 0)
                {
                    inBattle = false;
                }
            }

            GameManager.Instance.EndTurn();
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(20);
            Destroy(gameObject);
        }
    }
}
