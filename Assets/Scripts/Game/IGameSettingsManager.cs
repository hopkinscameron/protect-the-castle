namespace ProtectTheCastle.Game
{
    public interface IGameSettingsManager
    {
        BallPrefabSettings ballPrefabSettings { get; }
        CastlePrefabSettings castlePrefabSettings { get; }
        PlayerPrefabSettings playerPrefabSettings { get; }
        TowerPrefabSettings towerPrefabSettings { get; }
        bool loaded { get; }
    }
}
