using UnityEngine;

namespace ProtectTheCastle.Players
{
    public interface IPlayer
    {
        float damage { get; }
        float health { get; }
        float speed { get; }

        void Attack(GameObject playerToAttack);
        void Attacked(float amount);
        void HandleDeath();
        void Move();
        void SetHome(GameObject homeBase);
    }
}
