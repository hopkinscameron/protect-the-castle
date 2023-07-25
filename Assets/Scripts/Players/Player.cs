using System;
using ProtectTheCastle.Enums.Players;
using ProtectTheCastle.Environment.NavigationSpawns;
using ProtectTheCastle.Game;
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

        [SerializeField]
        private EnumPlayerType _type;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private GameObject _homeCastle;
        [SerializeField]
        private GameObject _lastTarget;
        [SerializeField]
        private GameObject _nextTarget;
        private bool _shouldMove;
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
            if (_shouldMove && _nextTarget)
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
        }

        public void HandleDeath()
        {
            Destroy(gameObject);
        }

        public bool Move()
        {
            var previousTarget = _nextTarget;
            _nextTarget = _nextTarget == null
                ? SpawnPlayerNavigationPoints.Instance.GetNextTarget(_homeCastle, transform.position)
                : SpawnPlayerNavigationPoints.Instance.GetNextTarget(_homeCastle, _nextTarget);
 
            var occupiedBy = _nextTarget.GetComponent<PlayerNavigationSpawn>().occupiedBy;
            if (occupiedBy != null)
            {
                Debug.Log(gameObject.name + " could not move because the target "
                    + _nextTarget.name + " at (" + _nextTarget.transform.position.x + "," + _nextTarget.transform.position.z
                    + ") is already occupied by " + occupiedBy.name);
                _nextTarget = previousTarget;
                return false;
            }

            Debug.Log(gameObject.name + " is moving towards " + _nextTarget.name + " at " + _nextTarget.transform.position.x + "," + _nextTarget.transform.position.z);
            _shouldMove = _navMeshAgent.SetDestination(_nextTarget.transform.position);
            return _shouldMove;
        }

        public void SetHome(GameObject homeBase)
        {
            _player1 = homeBase.tag.Equals(Constants.PLAYER_1_CASTLE_TAG, StringComparison.OrdinalIgnoreCase);
            _homeCastle = homeBase;
        }

        private PlayerPrefabTypeSettings GetSettings()
        {
            switch (_type)
            {
                case EnumPlayerType.Fire:
                    return GameSettingsManager.Instance.playerPrefabSettings.fire;
                case EnumPlayerType.Ice:
                    return GameSettingsManager.Instance.playerPrefabSettings.ice;
                case EnumPlayerType.Poison:
                    return GameSettingsManager.Instance.playerPrefabSettings.poison;
                case EnumPlayerType.Water:
                    return GameSettingsManager.Instance.playerPrefabSettings.water;
                default:
                    return GameSettingsManager.Instance.playerPrefabSettings.normal;
            }
        }

        private void MovePlayer()
        {
            _animator.SetFloat(Constants.ANIMATOR_SPEED_NAME, _navMeshAgent.velocity.magnitude);
            _shouldMove = !_navMeshAgentHelper.ReachedDestination(_navMeshAgent);

            if (!_shouldMove && ((GameManager.Instance.isPlayer1Turn && _player1) || (!GameManager.Instance.isPlayer1Turn && !_player1)))
            {
                _lastTarget = _nextTarget;
                _nextTarget = null;
                GameManager.Instance.EndTurn();
            }
        }
    }
}
