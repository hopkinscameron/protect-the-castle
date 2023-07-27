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
        bool inBattle { get; }

        void BeginBattle();
        void Attack();
        void Attacked(float amount);
        void HandleDeath();
        bool Move(EnumPlayerMoveDirection direction);
        void SetHome(GameObject homeBase);
    }
}
