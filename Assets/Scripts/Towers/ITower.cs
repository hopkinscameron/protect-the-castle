namespace ProtectTheCastle.Towers
{
    public interface ITower
    {
        float coolDown { get; }
        float health { get; }
        float healthDecreaseAmount { get; }
        float minEngageDistance { get; }

        void HandleDeath();
    }
}
