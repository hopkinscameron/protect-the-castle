using UnityEngine;

namespace ProtectTheCastle.Towers.Balls
{
    public interface IBall
    {
        float damage { get; }
        float speed { get; }

        void HandleDeath();
        void SetTarget(GameObject seekingTarget);
    }
}
