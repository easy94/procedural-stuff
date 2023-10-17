using Random = System.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public static class GenerateBiomes
{

    public static OozeType GenerateRndmBiomes(int mapsize, int gridX, int seed, int amountOfBiomes)
    {
        List<Vector3[]> hexGridVectors;
        List<Vector3[]> listOfNeighbours;
        Hexagon hexagonGrid = new();

        //flat top hexagon grid positions. each hexagon has its own vector array with the first element being the center point

        hexGridVectors = hexagonGrid.ConstructGrid(mapsize, gridX);


        // get neighbours of each hexagon here example: first hexagon has 2 neigbours aka the central points of the adjacent hexas
        listOfNeighbours = hexagonGrid.GetNeighboursPositions(hexGridVectors, mapsize, gridX);


        //hexgrid central points and its hexagon neigbours in one dictionary here


        //init oozetypes here********************************************
        int r = 12;

        OozeType oozeGrid = new(hexGridVectors,listOfNeighbours);


        for (int i = 0; i < r; i++)
        {
            oozeGrid.OozeProcess((i%amountOfBiomes) +1);
        }


        return oozeGrid;
    }

}