public interface IBaseCastleStats
{
    float health { get; }
    float numCastles { get; }
    float numPlayers { get; }

    void HandleDeath();
}
