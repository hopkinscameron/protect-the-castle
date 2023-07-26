using ProtectTheCastle.Shared;
using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public class PlayerNavigationSpawn : MonoBehaviour, IPlayerNavigationSpawn
    {
        public GameObject occupiedBy { get; private set; }
        public bool isDecisionSpawn { get; set; }
        public bool isPlayer1WinCondition { get; set; }
        public bool isPlayer2WinCondition { get; set; }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag.Equals(Constants.Player1.TAG)
                || collider.gameObject.tag.Equals(Constants.Player2.TAG))
            {
                // Debug.LogWarning(gameObject.name + " is occupied by " + collider.gameObject.name);
                occupiedBy = collider.gameObject;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag.Equals(Constants.Player1.TAG)
                || collider.gameObject.tag.Equals(Constants.Player2.TAG))
            {
                // Debug.LogWarning(gameObject.name + " is no longer occupied by " + collider.gameObject.name);
                occupiedBy = null;
            }
        }
    }
}
