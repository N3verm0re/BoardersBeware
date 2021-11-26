using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float gridHeight;
    private (int, int) cellSize;
    private int[,] grid;

    public Grid(int width, int height, float gridHeight, (int, int) cellSize)
    {
        this.width = width;
        this.height = height;
        this.gridHeight = gridHeight;
        this.cellSize = cellSize;

        grid = new int[width, height];
    }
}
