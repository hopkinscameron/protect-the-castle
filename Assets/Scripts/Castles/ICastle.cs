namespace ProtectTheCastle.Castles
{
    public interface ICastle
    {
        float health { get; }
        float numCastles { get; }
        float numPlayers { get; }

        void HandleDeath();
    }
}
