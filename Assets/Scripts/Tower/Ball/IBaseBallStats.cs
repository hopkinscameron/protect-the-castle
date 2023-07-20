public interface IBaseBallStats
{
    float damage { get; }
    float speed { get; }

    void HandleDeath();
}
