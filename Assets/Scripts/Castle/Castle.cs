public class Castle : BaseCastle
{
    public float castleHealth { get; private set; } = 10;
	public float numberOfCastles { get; private set; } = 3;
	public float numberOfPlayers { get; private set; } = 6;

	Castle()
    {
        base.health = castleHealth;
        base.numCastles = numberOfCastles;
        base.numPlayers = numberOfPlayers;
    }
}
