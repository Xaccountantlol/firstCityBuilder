using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;

    // Constructor that takes width and height as parameters
    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.Log(x + ", " + y);
            }
        }
    }

    // Method to visualize the grid using Gizmos (only visible in the Unity editor)
    public void DrawGizmos()
    {
        if (gridArray == null)
        {
            return;
        }

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Gizmos.DrawWireCube(GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector3(cellSize, cellSize, 0));
            }
        }
    }

    // Utility method to convert grid coordinates to world space coordinates
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
}
