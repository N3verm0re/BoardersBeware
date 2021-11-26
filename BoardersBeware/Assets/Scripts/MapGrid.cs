using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float gridHeight;
    private float cellSize;
    private int[,] grid;

    public Grid(int width, int height, float gridHeight, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.gridHeight = gridHeight;
        this.cellSize = cellSize;

        grid = new int[width, height];

        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, gridHeight, y), GetWorldPosition(x, gridHeight, y + 1), Color.blue, 100f);
                Debug.DrawLine(GetWorldPosition(x, gridHeight, y), GetWorldPosition(x + 1, gridHeight, y), Color.blue, 100f);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, float y, float z)
    {
        Vector3 returnVector = new Vector3(x, y);
        returnVector *= cellSize;
        returnVector.z = z;

        return returnVector;
    }
}
