using UnityEngine.AI;

namespace ProtectTheCastle.Shared
{
    public class NavMeshAgentHelper : INavMeshAgentHelper
    {
        public bool ReachedDestination(NavMeshAgent navMeshAgent)
        {
            // Check if we've reached the destination
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}