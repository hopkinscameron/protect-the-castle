using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private float speed = 7;
    private Vector3? targetPosition;

    void Start()
    {
        
    }

    public void setTarget(GameObject seekingTarget)
    {
        targetPosition = seekingTarget.transform.position;
    }

    private void FixedUpdate()
    {
        MoveTowardsTarget();
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
                Debug.Log("damage!");
                Destroy(this.gameObject);
                break;
            case "wall":
                Destroy(this.gameObject);
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
                Debug.Log("damage!");
                break;
            case "wall":
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }
}
