using System.Collections.Generic;
using UnityEngine;
public abstract class Grid
{
    //properties
    public int GridX { get; set; }
    public int GridY { get; set; }
    //used for calculating the radius of each cell
    public int Size { get; set; }

    //methods
    public abstract void ConstructGrid(int amountOfCols, int amountOfRows, Vector3 startPos);
    public abstract void SetNeighboursPositions();

}

