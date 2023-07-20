using UnityEngine;

public abstract class BaseTower : MonoBehaviour, IBaseTowerStats
{
    public float coolDown { get; set; }
    public float health { get; set; }
    public float healthDecreaseAmount { get; set; }
    public float minEngageDistance { get; set; }

    private GameObject ballPrefab;
    private GameObject player;
    private float timeSinceLastShot;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ballPrefab = (GameObject) Resources.Load("Prefabs/Cannon Ball");
    }

    void FixedUpdate()
    {
        health = health - healthDecreaseAmount * Time.deltaTime;
        if (health <= 0)
        {
            HandleDeath();
        }

        if (player == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < minEngageDistance)
        {
            if (timeSinceLastShot <= Time.time)
            {
                Debug.Log("Fire!");
                GameObject spawnedBall = Instantiate(ballPrefab, transform.position, transform.rotation);
                spawnedBall.GetComponent<BallMovement>().SetTarget(player);
                timeSinceLastShot = Time.time + coolDown;
            }
        }
    }

    public void HandleDeath()
	{
		Destroy(gameObject);
	}
}
