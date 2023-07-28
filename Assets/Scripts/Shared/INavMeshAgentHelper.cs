using UnityEngine.AI;

namespace ProtectTheCastle.Shared
{
    public interface INavMeshAgentHelper
    {
        bool ReachedDestination(NavMeshAgent navMeshAgent);
    }
}