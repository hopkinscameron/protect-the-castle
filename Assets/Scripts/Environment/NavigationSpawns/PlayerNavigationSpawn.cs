using System.Collections.Generic;
using ProtectTheCastle.Shared;
using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public class PlayerNavigationSpawn : MonoBehaviour, IPlayerNavigationSpawn
    {
        public List<GameObject> occupiedBy { get; private set; } = new List<GameObject>();
        public bool isDecisionSpawn { get; set; }
        public bool isPlayer1WinCondition { get; set; }
        public bool isPlayer2WinCondition { get; set; }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag.Equals(Constants.Player1.TAG)
                || collider.gameObject.tag.Equals(Constants.Player2.TAG))
            {
                occupiedBy.Add(collider.gameObject);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag.Equals(Constants.Player1.TAG)
                || collider.gameObject.tag.Equals(Constants.Player2.TAG))
            {
                occupiedBy.Remove(collider.gameObject);
            }
        }
    }
}
