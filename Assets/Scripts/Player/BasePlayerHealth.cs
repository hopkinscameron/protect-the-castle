using UnityEngine;

public class BasePlayerHealth : MonoBehaviour
{
    public float health { get; private set; } = 100;

    public void PlayerAttacked(float amount)
    {
        health = health - amount;
    }

    private void HandleDeath()
    {
        // TODO: play animation
        Destroy(this.gameObject);
    }
}
