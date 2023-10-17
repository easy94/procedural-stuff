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

    public static List<OozeType> GenerateRndmBiomes(int mapsize, int gridX, int seed)
    {
        List<Vector3[]> hexGridVectors;
        List<Vector3[]> listOfNeighbours;
        Hexagon hexagonGrid = new();

        //flat top hexagon grid positions. each hexagon has its own vector array with the first element being the center point

        hexGridVectors = hexagonGrid.ConstructGrid(mapsize, gridX);


        // get neighbours of each hexagon here example: first hexagon has 2 neigbours aka the central points of the adjacent hexas
        listOfNeighbours = hexagonGrid.GetNeighboursPositions(hexGridVectors, mapsize, gridX);


        //hexgrid central points and its hexagon neigbours in one dictionary here
        List<OozeType> r_dict = new();

        //init oozetypes here********************************************
        int r = 12;

        OozeType[] oozeInstances = new OozeType[r];


        for (int i = 0; i < r; i++)
        {
            oozeInstances[i] = new OozeType(hexGridVectors, listOfNeighbours);
            r_dict.Add(oozeInstances[i].OozeProcess(seed));

        }

        return r_dict;
    }

}