public class Tower : BaseTower
{
    public float towerCoolDown { get; private set; } = 5;
    public float towerHealth { get; private set; } = 5;
    public float towerHealthDecreaseAmount { get; private set; } = 1;
    public float minimumEngageDistance { get; private set; } = 20;

    Tower()
    {
        base.coolDown = towerCoolDown;
        base.health = towerHealth;
        base.healthDecreaseAmount = towerHealthDecreaseAmount;
        base.minEngageDistance = minimumEngageDistance;
    }
}
