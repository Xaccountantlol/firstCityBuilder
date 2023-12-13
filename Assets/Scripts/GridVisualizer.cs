using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the grid with width, height, and cell size
        int width = 10; // Set the width of the grid
        int height = 5; // Set the height of the grid
        float cellSize = 1f; // Set the size of each cell in the grid
        grid = new Grid(width, height, cellSize);

        if (cellPrefab == null)
        {
            Debug.LogError("Cell prefab is not assigned in the inspector!");
            return;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Instantiate a prefab at each grid position
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                Instantiate(cellPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}