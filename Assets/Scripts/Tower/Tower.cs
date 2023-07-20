using UnityEngine;

public class Tower : MonoBehaviour
{
    float minEngageDistance = 20;
    float coolDown = 5;

    [SerializeField]
    private GameObject ballPrefab;
    private GameObject player;
    private float timeSinceLastShot;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        // Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) < minEngageDistance)
        {
            if (timeSinceLastShot <= Time.time)
            {
                Debug.Log("Fire!");
                GameObject spawnedBall = Instantiate(ballPrefab, transform.position, transform.rotation);
                spawnedBall.GetComponent<BallMovement>().setTarget(player);
                timeSinceLastShot = Time.time + coolDown;
            }
        }
    }
}
