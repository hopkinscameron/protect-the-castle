using ProtectTheCastle.Shared;
using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public class PlayerNavigationSpawn : MonoBehaviour, IPlayerNavigationSpawnPoint
    {
        public GameObject occupiedBy { get; private set; }
        public bool isDecisionSpawn { get; set; }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag.Equals(Constants.PLAYER_1_TAG)
                || collider.gameObject.tag.Equals(Constants.PLAYER_2_TAG))
            {
                // Debug.LogWarning(gameObject.name + " is occupied by " + collider.gameObject.name);
                occupiedBy = collider.gameObject;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag.Equals(Constants.PLAYER_1_TAG)
                || collider.gameObject.tag.Equals(Constants.PLAYER_2_TAG))
            {
                // Debug.LogWarning(gameObject.name + " is no longer occupied by " + collider.gameObject.name);
                occupiedBy = null;
            }
        }
    }
}
