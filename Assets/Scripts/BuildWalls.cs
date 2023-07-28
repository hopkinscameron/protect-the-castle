using System.Collections.Generic;
using UnityEngine;

public class BuildWalls : MonoBehaviour
{
    [SerializeField]
    private GameObject block1;
    [SerializeField]
    private GameObject block2;
    private int overallLimit = 0;
    private List<(int, int)> gridDirections = new List<(int, int)>{ (6, 14), (10, -10), (4, 8), (6, -4), (6, 6), (8, -14), (11, 0) };
    private List<(int, int)> gridDirections2 = new List<(int, int)>{ (0, 11), (4, -11), (10, 9), (0, -5), (12, 6), (2, -11) };
    private List<(int, int)> gridDirections3 = new List<(int, int)>{ (6, -6), (16, 3), (4, -12), (-4, -4), (8, -3), (10, 6), (2, 17), (9, 0) };
    private List<(int, int)> gridDirections4 = new List<(int, int)>{ (0, -3), (4, 3), (2, -3), (4, 3), (10, -16), (4, -3), (4, 6), (2, 14) };
    private List<(int, int)> gridDirections5 = new List<(int, int)>{ (0, 4) };

    private void Start()
    {
        loadBlocks(gridDirections, 54f, 0f, true);
        overallLimit = 0;
        loadBlocks(gridDirections2, 54f, 18f, true);
        overallLimit = 0;
        loadBlocks(gridDirections3, 46f, 0f, true);
        overallLimit = 0;
        loadBlocks(gridDirections4, 46f, 18f, true);
        overallLimit = 0;
        loadBlocks(gridDirections5, 54f, 59f, true);
    }

    private void loadBlocks(List<(int, int)> grid, float lastX, float lastZ, bool b1)
    {
        if (overallLimit == grid.Count) return;

        var numBlocks = grid[overallLimit];
        overallLimit++;
        var zDir = numBlocks.Item1 > 0 ? 1 : -1;
        var zBlocks = numBlocks.Item1 * zDir;
        for (var z = 1; z <= zBlocks; z++)
        {
            Instantiate(b1 ? block1 : block2, new Vector3(lastX, 1.08f, lastZ), transform.rotation);
            lastZ += (2 * zDir);
            b1 = !b1;
        }
        
        var xDir = numBlocks.Item2 > 0 ? 1 : -1;
        var xBlocks = numBlocks.Item2 * xDir;
        for (var z = 1; z <= xBlocks; z++)
        {
            Instantiate(b1 ? block1 : block2, new Vector3(lastX, 1.08f, lastZ), transform.rotation);
            lastX += (2 * xDir);
            b1 = !b1;
        }

        loadBlocks(grid, lastX, lastZ, b1);
    }
}
