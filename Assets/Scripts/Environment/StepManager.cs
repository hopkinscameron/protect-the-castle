using UnityEngine;

public class StepManager : MonoBehaviour
{
    private GameObject[] _steps;

    void Start()
    {
        _steps = GameObject.FindGameObjectsWithTag("Step");
    }

    public GameObject GetNextClosestStep(Vector3 position, Vector3 homeCastlePosition, int positiveDirection)
    {
        foreach (GameObject step in _steps)
        {
            Vector3 stepPostition = step.transform.position;
            var dist = Mathf.Abs(position.z - stepPostition.z);
            if (dist <= 5 && IsMovingForward(stepPostition, position, homeCastlePosition, positiveDirection))
            {
                return step;
            }
        }

        return null;
    }

    private bool IsMovingForward(Vector3 nextStepPostition, Vector3 currentPosition, Vector3 homeCastlePosition, int positiveDirection)
    {
        if (positiveDirection == -1)
        {
            return nextStepPostition.z - homeCastlePosition.z < currentPosition.z - homeCastlePosition.z;
        }

        return nextStepPostition.z - homeCastlePosition.z > currentPosition.z - homeCastlePosition.z;
    }
}
