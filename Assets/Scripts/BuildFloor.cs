using UnityEngine;

public class BuildFloor : MonoBehaviour
{
    [SerializeField]
    private GameObject ground1;
    [SerializeField]
    private GameObject ground2;

    private void Start()
    {
        bool g1 = true;
        for (var z = 0; z <= 100; z += 2)
        {
            for (var x = 0; x <= 100; x += 2)
            {
                Instantiate(g1 ? ground1 : ground2, new Vector3(x, 0, z), transform.rotation);
                g1 = !g1;
            }
        }
    }
}
