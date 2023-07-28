using System.Collections;
using ProtectTheCastle.Environment.AISpawns;
using ProtectTheCastle.Game;
using ProtectTheCastle.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace ProtectTheCastle.AI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIWander : MonoBehaviour, IAIWander
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        [SerializeField]
        private GameObject _target;
        private bool _shouldMove;
        private INavMeshAgentHelper _navMeshAgentHelper;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _navMeshAgentHelper = new NavMeshAgentHelper();
        }

        private void Start()
        {
            StartCoroutine(IdleAI());
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.gameInProgress) return;
            
            if (_shouldMove && _target)
            {
                MoveAI();
            }
            else if (!_shouldMove)
            {
                _shouldMove = true;
                StartCoroutine(IdleAI());
            }
        }

        private void SetTarget()
        {
            _target = _target == null
                ? AISpawnManager.Instance.GetNextTarget(transform.position)
                : AISpawnManager.Instance.GetNextTarget(_target);
            _navMeshAgent.destination = _target.transform.position;
        }

        private void MoveAI()
        {
            _animator.SetFloat(Constants.Animations.SPEED_NAME, _navMeshAgent.velocity.magnitude);
            _shouldMove = !_navMeshAgentHelper.ReachedDestination(_navMeshAgent);
        }

        private IEnumerator IdleAI()
        {
            var waitForSeconds = Random.Range(1, 15);
            yield return new WaitForSeconds(0);
            SetTarget();
        }
    }
}
