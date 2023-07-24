namespace ProtectTheCastle.Game
{
    public interface IGameSettingsManager
    {
        bool gameInProgress { get; }
        bool pickingPlayers { get; }
        bool pickingTowers { get; }
        bool isPlayer1Turn { get; }

        bool StartGame();
        bool StartPickingPlayers();
        bool StartPickingTowers();
    }
}
