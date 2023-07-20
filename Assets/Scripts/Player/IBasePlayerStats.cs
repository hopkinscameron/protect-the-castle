public interface IBasePlayerStats
{
    float damage { get; }
    float health { get; }
    float speed { get; }

    void Attacked(float amount);
    void HandleDeath();
}
