using UnityEngine;

namespace ProtectTheCastle.Tower.Balls
{
    public interface IBall
    {
        float damage { get; }
        float speed { get; }

        void HandleDeath();
        void SetTarget(GameObject seekingTarget);
    }
}
