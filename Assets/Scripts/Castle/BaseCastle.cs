using UnityEngine;

public abstract class BaseCastle : MonoBehaviour, IBaseCastleStats
{
	public float health { get; set; }
	public float numCastles { get; set; }
	public float numPlayers { get; set; }

    public bool Attacked(float amount)
    {
        health = health - amount;
        return health <= 0;
    }

    public void HandleDeath()
	{
		Destroy(gameObject);
	}
}
