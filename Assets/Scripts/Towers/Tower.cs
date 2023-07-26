using System.Collections.Generic;
using System.Linq;
using ProtectTheCastle.Enums.Towers;
using ProtectTheCastle.Game;
using ProtectTheCastle.Players;
using ProtectTheCastle.Shared;
using ProtectTheCastle.Towers.Balls;
using UnityEngine;

namespace ProtectTheCastle.Towers
{
    public class Tower : MonoBehaviour, ITower
    {
        public float coolDown { get; private set; }
        public float health { get; private set; }
        public float healthDecreaseAmount { get; private set; }
        public float minEngageDistance { get; private set; }

        [SerializeField]
        private EnumTowerType _type;
        [SerializeField]
        private GameObject _muzzle;
        [SerializeField]
        private GameObject _ballPrefab;
        [SerializeField]
        private List<GameObject> _playersToAttack;
        [SerializeField]
        private GameObject _currentTarget;
        private IPlayer _currentTargetPlayerScript;
        private float _timeSinceLastShot;
        private bool _isPlayerTower;

        private void Awake()
        {
            _isPlayerTower = gameObject.tag.Equals(Constants.Player1.CASTLE_TAG, System.StringComparison.OrdinalIgnoreCase);
        }

        private void Start()
        {
            TowerPrefabTypeSettings settings = GetSettings();
            coolDown = settings.coolDown;
            health = settings.health;
            healthDecreaseAmount = settings.healthDecreaseAmount;
            minEngageDistance = settings.minEngageDistance;
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.gameInProgress) return;
            SetPlayers();
            SearchOrEngage();
        }

        public void HandleDeath()
        {
            Destroy(gameObject);
        }

        private TowerPrefabTypeSettings GetSettings()
        {
            switch (_type)
            {
                case EnumTowerType.Fire:
                    return GameSettingsManager.Instance.towerPrefabSettings.fire;
                case EnumTowerType.Ice:
                    return GameSettingsManager.Instance.towerPrefabSettings.ice;
                case EnumTowerType.Poison:
                    return GameSettingsManager.Instance.towerPrefabSettings.poison;
                case EnumTowerType.Water:
                    return GameSettingsManager.Instance.towerPrefabSettings.water;
                default:
                    return GameSettingsManager.Instance.towerPrefabSettings.normal;
            }
        }

        private void SetPlayers()
        {
            if (_playersToAttack.Count == 0 && _isPlayerTower)
            {
                _playersToAttack = GameObject.FindGameObjectsWithTag(Constants.Player2.TAG).ToList();
            }
            else if (_playersToAttack.Count == 0)
            {
                _playersToAttack = GameObject.FindGameObjectsWithTag(Constants.Player1.TAG).ToList();
            }
        }

        private void SearchOrEngage()
        {
            if (_currentTarget != null)
            {
                if (IsTargetWithinDistance(_currentTarget) && _currentTargetPlayerScript.alive && _currentTargetPlayerScript.moving)
                {
                    health = health - healthDecreaseAmount * Time.deltaTime;
                    if (health <= 0)
                    {
                        HandleDeath();
                    }
                    else if (_timeSinceLastShot <= Time.time)
                    {
                        var spawnedBall = Instantiate(_ballPrefab, _muzzle.transform.position, _muzzle.transform.rotation);
                        (spawnedBall.GetComponent(typeof(IBall)) as IBall).SetTarget(_currentTarget);
                        _timeSinceLastShot = Time.time + coolDown;
                    }
                }
                else
                {
                    _currentTarget = null;
                }
            }
            else
            {
                GetPlayerTarget();
            }
        }

        private void GetPlayerTarget()
        {
            if (_currentTarget != null) return;

            foreach (var player in _playersToAttack)
            {
                if (player != null && IsTargetWithinDistance(player))
                {
                    var playerScript = (player.GetComponent(typeof(IPlayer)) as IPlayer);
                    if (playerScript.alive && playerScript.moving) 
                    {
                        _currentTargetPlayerScript = playerScript;
                        _currentTarget = player;
                    }
                }
            }
        }

        private bool IsTargetWithinDistance(GameObject target)
        {
            return Vector3.Distance(_muzzle.transform.position, target.transform.position) < minEngageDistance;
        }
    }
}
