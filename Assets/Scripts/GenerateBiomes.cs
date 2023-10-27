using System.Collections.Generic;
using UnityEngine;

public static class GenerateBiomes
{

    public static OozeGrid GenerateRndmBiomes(int mapsize, int gridX, int amountOfBiomes)
    {
        OozeGrid hexagonGrid = new(mapsize, gridX);

        //init oozetypes here********************************************
        int r = 8;

        for (int i = 0; i < r; i++)
        {
            hexagonGrid.OozeProcess((i%amountOfBiomes) +1);
        }

        return hexagonGrid;
    }

}