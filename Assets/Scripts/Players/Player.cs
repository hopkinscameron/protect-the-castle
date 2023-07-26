using System;
using System.Collections;
using ProtectTheCastle.Environment.NavigationSpawns;
using ProtectTheCastle.Game;
using ProtectTheCastle.Players.Enums;
using ProtectTheCastle.Shared;
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

        [SerializeField]
        private EnumPlayerType _type;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private GameObject _homeCastle;
        [SerializeField]
        private GameObject _lastTarget;
        [SerializeField]
        private GameObject _nextTarget;
        private INavMeshAgentHelper _navMeshAgentHelper;
        private bool _player1;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _navMeshAgentHelper = new NavMeshAgentHelper();
        }

        private void Start()
        {
            PlayerPrefabTypeSettings settings = GetSettings();
            damage = settings.damage;
            health = settings.health;
            speed = settings.speed;
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.gameInProgress || !alive) return;

            if (moving && _nextTarget)
            {
                MovePlayer();
            }
        }

        public void Attack(GameObject playerToAttack)
        {
            throw new System.NotImplementedException();
        }

        public void Attacked(float amount)
        {
            health = health - amount;

            if (health <= 0)
            {
                HandleDeath();
            }
            else
            {
                _animator.SetTrigger(Constants.Animations.ANIMATOR_GET_HIT_NAME);
            }
        }

        public void HandleDeath()
        {
            alive = false;
            _animator.SetTrigger(Constants.Animations.ANIMATOR_DIE_NAME);
            StartCoroutine("Die");
        }

        public bool Move(EnumPlayerMoveDirection direction)
        {
            if (!alive) return false;

            var previousTarget = _nextTarget;
            _nextTarget = _nextTarget == null
                ? SpawnPlayerNavigationPoints.Instance.GetNextTarget(_homeCastle, transform.position, direction)
                : SpawnPlayerNavigationPoints.Instance.GetNextTarget(_homeCastle, _nextTarget, direction);

            if (_nextTarget == null)
            {
                return false;
            }

            var occupiedBy = (_nextTarget.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn).occupiedBy;
            if (occupiedBy != null)
            {
                Debug.Log(gameObject.name + " could not move because the target "
                    + _nextTarget.name + " at (" + _nextTarget.transform.position.x + "," + _nextTarget.transform.position.z
                    + ") is already occupied by " + occupiedBy.name);
                _nextTarget = previousTarget;
                return false;
            }

            Debug.Log(gameObject.name + " is moving towards " + _nextTarget.name + " at " + _nextTarget.transform.position.x + "," + _nextTarget.transform.position.z);
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
            _animator.SetFloat(Constants.Animations.ANIMATOR_SPEED_NAME, _navMeshAgent.velocity.magnitude);
            moving = !_navMeshAgentHelper.ReachedDestination(_navMeshAgent);

            if (!moving && ((GameManager.Instance.isPlayer1Turn && _player1) || (!GameManager.Instance.isPlayer1Turn && !_player1)))
            {
                _lastTarget = _nextTarget;
                _nextTarget = null;

                var pns = (_lastTarget.GetComponent(typeof(IPlayerNavigationSpawn)) as IPlayerNavigationSpawn);
                if ((_player1 && pns.isPlayer1WinCondition) || (!_player1 && pns.isPlayer2WinCondition))
                {
                    GameManager.Instance.EndGame(gameObject);
                    _animator.SetTrigger(Constants.Animations.ANIMATOR_VICTORY_NAME);
                    return;
                }

                GameManager.Instance.EndTurn();
            }
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(20);
            Destroy(this.gameObject);
        }
    }
}
