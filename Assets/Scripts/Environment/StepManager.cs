using UnityEngine;

public class StepManager : MonoBehaviour
{
    private GameObject[] steps;

    void Start()
    {
        steps = GameObject.FindGameObjectsWithTag("Step");
    }

    public GameObject GetNextClosestStep(Vector3 position, Vector3 homeCastlePosition)
    {
        foreach (GameObject step in steps)
        {
            Vector3 stepPostition = step.transform.position;
            var dist = Mathf.Abs(position.z - stepPostition.z);
            if (dist <= 5 && IsMovingForward(stepPostition, position, homeCastlePosition))
            {
                return step;
            }
        }

        return null;
    }

    private bool IsMovingForward(Vector3 nextStepPostition, Vector3 currentPosition, Vector3 homeCastlePosition)
    {
        return nextStepPostition.z - homeCastlePosition.z > currentPosition.z - homeCastlePosition.z;
    }
}
