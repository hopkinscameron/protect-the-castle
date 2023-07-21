using ProtectTheCastle.Game;
using ProtectTheCastle.Players;
using ProtectTheCastle.Tower.Enums.Balls;
using UnityEngine;

namespace ProtectTheCastle.Tower.Balls
{
    public class Ball : MonoBehaviour, IBall
    {
        public float speed { get; private set; }
        public float damage { get; private set; }
        
        [SerializeField]
        private EnumBallType _type;
        private Vector3? _targetPosition;

        void Start()
        {
            BallPrefabTypeSettings settings = GetSettings();
            speed = settings.speed;
            damage = settings.damage;
        }

        void FixedUpdate()
        {
            MoveTowardsTarget();
        }

        public void HandleDeath()
        {
            Destroy(gameObject);
        }

        public void SetTarget(GameObject seekingTarget)
        {
            _targetPosition = seekingTarget.transform.position;
        }

        private BallPrefabTypeSettings GetSettings()
        {
            switch (_type)
            {
                case EnumBallType.Fire:
                    return GameSettingsManager.Instance.ballPrefabSettings.fire;
                case EnumBallType.Ice:
                    return GameSettingsManager.Instance.ballPrefabSettings.ice;
                case EnumBallType.Poison:
                    return GameSettingsManager.Instance.ballPrefabSettings.poison;
                case EnumBallType.Water:
                    return GameSettingsManager.Instance.ballPrefabSettings.water;
                default:
                    return GameSettingsManager.Instance.ballPrefabSettings.normal;
            }
        }

        private void MoveTowardsTarget()
        {
            if (_targetPosition != null)
            {
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition.Value, step);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var collidedTag = other.gameObject.tag.ToLower();
            switch (collidedTag)
            {
                case "player":
                    HandleHit(other.gameObject);
                    HandleDeath();
                    break;
                case "wall":
                    HandleDeath();
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            var collidedTag = other.gameObject.tag.ToLower();
            switch (collidedTag)
            {
                case "player":
                    HandleHit(other.gameObject);
                    HandleDeath();
                    break;
                case "wall":
                    HandleDeath();
                    break;
                default:
                    break;
            }
        }

        private void HandleHit(GameObject playerHit)
        {
            playerHit.GetComponent<Player>().Attacked(damage);
        }
    }
}
