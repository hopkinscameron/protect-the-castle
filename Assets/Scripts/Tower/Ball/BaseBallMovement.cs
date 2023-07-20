using UnityEngine;

public abstract class BaseBallMovement : MonoBehaviour, IBaseBallStats, IBaseBaseMovement
{
    public float speed { get; set; }
    public float damage { get; set; }

    private Vector3? targetPosition;

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    public void HandleDeath()
    {
        Destroy(gameObject);
    }

    public void SetTarget(GameObject seekingTarget)
    {
        targetPosition = seekingTarget.transform.position;
    }

    private void MoveTowardsTarget()
    {
        if (targetPosition != null)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, step);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var collidedTag = other.gameObject.tag.ToLower();
        switch (collidedTag)
        {
            case "player":
                HandleHit(other.gameObject);
                HandleDeath();
                break;
            case "wall":
                HandleDeath();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        var collidedTag = other.gameObject.tag.ToLower();
        switch (collidedTag)
        {
            case "player":
                HandleHit(other.gameObject);
                HandleDeath();
                break;
            case "wall":
                HandleDeath();
                break;
            default:
                break;
        }
    }

    private void HandleHit(GameObject playerHit)
    {
        playerHit.GetComponent<PlayerMovement>().Attacked(damage);
    }
}
