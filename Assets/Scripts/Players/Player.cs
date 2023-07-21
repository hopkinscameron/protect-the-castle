using ProtectTheCastle.Enums.Players;
using ProtectTheCastle.Game;
using UnityEngine;

namespace ProtectTheCastle.Players
{
    public class Player : MonoBehaviour, IPlayer
    {
        public float damage { get; private set; }
        public float health { get; private set; }
        public float speed { get; private set; }

        [SerializeField]
        private EnumPlayerType _type;
        private bool _shouldMove;
        private Vector3 _homeCastlePosition;
        private StepManager _stepManager;
        private GameObject _targetStep;
        private bool _isPlayer;

        void Awake()
        {
            _isPlayer = gameObject.tag.Equals("Player", System.StringComparison.OrdinalIgnoreCase);
            _stepManager = GameObject.Find("Steps").GetComponent<StepManager>();

            if (_isPlayer)
            {
                _homeCastlePosition = GameObject.FindGameObjectWithTag("Player Castle").transform.position;
            }
            else
            {
                _homeCastlePosition = GameObject.FindGameObjectWithTag("Enemy Castle").transform.position;
            }
        }

        void Start()
        {
            PlayerPrefabTypeSettings settings = GetSettings();
            damage = settings.damage;
            health = settings.health;
            speed = settings.speed;

            Invoke("RestartMovement", 1);
        }

        void FixedUpdate()
        {
            if (_shouldMove)
            {
                if (_targetStep == null)
                {
                    _targetStep = _stepManager.GetNextClosestStep(transform.position, _homeCastlePosition, _isPlayer ? 1 : -1);
                }
                else
                {
                    MovePlayer();
                }
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

        public void Move()
        {
            _shouldMove = true;
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

        private void RestartMovement()
        {
            Move();
        }

        private void MovePlayer()
        {
            if (_targetStep != null)
            {
                var step = speed * Time.deltaTime;
                var onlyOneAxisPosition = new Vector3(transform.position.x, transform.position.y, _targetStep.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, onlyOneAxisPosition, step);
                if (Vector3.Distance(transform.position, onlyOneAxisPosition) < 0.001f)
                {
                    if (_targetStep.name.ToLower() != "ai home step")
                    {
                        _shouldMove = false;
                        _targetStep = null;
                        Invoke("RestartMovement", 1);
                    }
                    else
                    {
                        this._shouldMove = false;
                        Debug.Log("won!");
                    }
                }
            }
        }
    }
}
