public class BallMovement : BaseBallMovement
{
    public float attackingDamage { get; private set; } = 200;
    public float attackingSpeed { get; private set; } = 20;

    BallMovement()
    {
        base.damage = attackingDamage;
        base.speed = attackingSpeed;
    }
}
