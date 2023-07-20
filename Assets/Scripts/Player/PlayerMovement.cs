public class PlayerMovement : BasePlayerMovement
{
    public float attackingDamage { get; private set; } = 15;
    public float playerHealth { get; private set; } = 100;
    public float playerMovementSpeed { get; private set; } = 1;

    PlayerMovement()
    {
        base.damage = attackingDamage;
        base.health = playerHealth;
        base.speed = playerMovementSpeed;
    }

    void Start()
    {
        Invoke("RestartMovement", 1);
    }

    private void RestartMovement()
    {
        Move();
    }
}
