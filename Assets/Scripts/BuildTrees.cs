using UnityEngine;

public class BuildTrees : MonoBehaviour
{
    [SerializeField]
    private GameObject trees1;

    private void Start()
    {
        for (var z = 0; z <= 100; z += 2)
        {
            for (var x = 0; x <= 100; x += 2)
            {
                Instantiate(trees1, new Vector3(x, 1.05f, z), transform.rotation);
            }
        }
    }
}
