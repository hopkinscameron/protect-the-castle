public interface IBaseTowerStats
{
    float coolDown { get; }
    float health { get; }
    float healthDecreaseAmount { get; }
    float minEngageDistance { get; }

    void HandleDeath();
}
