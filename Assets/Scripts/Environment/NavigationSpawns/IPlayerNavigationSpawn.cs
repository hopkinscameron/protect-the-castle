using System.Collections.Generic;
using UnityEngine;

namespace ProtectTheCastle.Environment.NavigationSpawns
{
    public interface IPlayerNavigationSpawn
    {
        List<GameObject> occupiedBy { get; }
        bool isDecisionSpawn { get; set; }
        bool isPlayer1WinCondition { get; set; }
        bool isPlayer2WinCondition { get; set; }
    }
}
