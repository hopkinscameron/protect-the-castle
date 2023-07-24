using ProtectTheCastle.Enums.Towers;
using ProtectTheCastle.Game;
using ProtectTheCastle.Tower.Balls;
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
        private GameObject _ballPrefab;
        private GameObject _player;
        private float _timeSinceLastShot;
        private bool _isPlayerTower;

        private void Awake()
        {
            _isPlayerTower = gameObject.tag.Equals("Player Tower", System.StringComparison.OrdinalIgnoreCase);

            if (_isPlayerTower)
            {
                _player = GameObject.FindGameObjectWithTag("Enemy");
            }
            else
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
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
            health = health - healthDecreaseAmount * Time.deltaTime;
            if (health <= 0)
            {
                HandleDeath();
            }

            if (_player == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, _player.transform.position) < minEngageDistance)
            {
                if (_timeSinceLastShot <= Time.time)
                {
                    Debug.Log("Fire!");
                    GameObject spawnedBall = Instantiate(_ballPrefab, transform.position, transform.rotation);
                    IBall spawnedBallMovementScript = spawnedBall.GetComponent<Ball>();
                    spawnedBallMovementScript.SetTarget(_player);
                    _timeSinceLastShot = Time.time + coolDown;
                }
            }
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
    }
}
