using System;
using ProtectTheCastle.Game;
using ProtectTheCastle.Players;
using ProtectTheCastle.Shared;
using ProtectTheCastle.Towers.Enums.Balls;
using UnityEngine;

namespace ProtectTheCastle.Towers.Balls
{
    public class Ball : MonoBehaviour, IBall
    {
        public float speed { get; private set; }
        public float damage { get; private set; }
        
        [SerializeField]
        private EnumBallType _type;
        private GameObject _target;
        private float _tagetCenter;

        private void Awake()
        {
            BallPrefabTypeSettings settings = GetSettings();
            speed = settings.speed;
            damage = settings.damage;
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.gameInProgress) return;

            MoveTowardsTarget();
        }

        public void HandleDeath()
        {
            Destroy(gameObject);
        }

        public void SetTarget(GameObject seekingTarget)
        {
            _tagetCenter = seekingTarget.GetComponent<CharacterController>().center.y;
            _target = seekingTarget;
            Debug.Log(_tagetCenter);
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
            if (_target != null)
            {
                var step = speed * Time.deltaTime;
                var targetPosition = _target.transform.position;
                var newTargetPosition = new Vector3(targetPosition.x, _tagetCenter, targetPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, newTargetPosition, step);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("OnCollisionEnter: " + other.gameObject.name);

            if (other.gameObject == _target)
            {
                HandleTargetHit();
            }
            
            HandleDeath();
        }

        private void OnTriggerEnter(Collider other) 
        {
            Debug.Log("OnTriggerEnter: " + other.gameObject.name);

            if (other.gameObject == _target)
            {
                HandleTargetHit();
            }
            
            HandleDeath();
        }

        private void HandleTargetHit()
        {
            (_target.GetComponent(typeof(IPlayer)) as IPlayer).Attacked(damage);
        }
    }
}
