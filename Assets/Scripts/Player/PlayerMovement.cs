using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool shouldMove = false;
    private float speed = 1.0f;
    private Vector3 homeCastlePosition;
    private StepManager stepManager;
    private GameObject targetStep;

    void Start()
    {
        stepManager = GameObject.Find("Steps").GetComponent<StepManager>();
        homeCastlePosition = GameObject.Find("Player Castle").transform.position;
        Invoke("RestartMovement", 1);
    }
    
    private void FixedUpdate()
    {
        if (shouldMove)
        {
            if (targetStep == null)
            {
                targetStep = stepManager.GetNextClosestStep(transform.position, homeCastlePosition);
            }
            else
            {
                Move();
            }
        }
    }

    private void Move()
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
                    Debug.Log("won!");
                }
            }
        }
    }

    private void RestartMovement()
    {
        shouldMove = true;
    }
}
