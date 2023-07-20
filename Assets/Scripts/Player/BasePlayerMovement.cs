using UnityEngine;

public abstract class BasePlayerMovement : MonoBehaviour, IBasePlayerStats, IBasePlayerMovement
{
    public float health { get; set; }
    public float speed { get; set; }
    public float damage { get; set; }

    private bool shouldMove { get; set; }
    private Vector3 homeCastlePosition;
    private StepManager stepManager;
    private GameObject targetStep;

    void Awake()
    {
        stepManager = GameObject.Find("Steps").GetComponent<StepManager>();
        homeCastlePosition = GameObject.Find("Player Castle").transform.position;
    }

    void FixedUpdate()
    {
        if (shouldMove)
        {
            if (targetStep == null)
            {
                targetStep = stepManager.GetNextClosestStep(transform.position, homeCastlePosition);
            }
            else
            {
                MovePlayer();
            }
        }
    }

	public void Attack(GameObject playerToAttack)
	{
		throw new System.NotImplementedException();
	}

	public void Attacked(float amount)
    {
        health = health - amount;

        if (health <= 0)
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
	{
		Destroy(gameObject);
	}

	public void Move()
	{
		shouldMove = true;
	}

    private void MovePlayer()
    {
        if (targetStep != null)
        {
            var step = speed * Time.deltaTime;
            var onlyOneAxisPosition = new Vector3(transform.position.x, transform.position.y, targetStep.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, onlyOneAxisPosition, step);
            if (Vector3.Distance(transform.position, onlyOneAxisPosition) < 0.001f)
            {
                if (targetStep.name.ToLower() != "ai home step")
                {
                    shouldMove = false;
                    targetStep = null;
                    Invoke("RestartMovement", 1);
                }
                else
                {
                    this.shouldMove = false;
                    Debug.Log("won!");
                }
            }
        }
    }
}
