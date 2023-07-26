using ProtectTheCastle.Players.Enums;
using UnityEngine;

namespace ProtectTheCastle.Players
{
    public interface IPlayer
    {
        float damage { get; }
        float health { get; }
        float speed { get; }
        bool alive { get; }
        bool moving { get; }

        void Attack(GameObject playerToAttack);
        void Attacked(float amount);
        void HandleDeath();
        bool Move(EnumPlayerMoveDirection direction);
        void SetHome(GameObject homeBase);
    }
}
