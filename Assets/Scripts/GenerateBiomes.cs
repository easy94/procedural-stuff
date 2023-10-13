using Random = System.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Rendering;
using System.Threading;
using UnityEditor;

public static class GenerateBiomes
{


    public static Dictionary<int, List<Vector3>> GenerateRndmBiomes(Biomes biome, int gridX, int gridY, int seed)
    {
        List<Vector3[]> hexGridVectors;
        List<Vector3[]> listOfNeighbours;
        Hexagon hexagonGrid = new(gridX, gridY);

        //flat top hexagon grid positions. each hexagon has its own vector array with the first element being the center point

        hexGridVectors = hexagonGrid.ConstructGrid(gridX);


        // get neighbours of each hexagon here example: first hexagon has 2 neigbours aka the central points of the adjacent hexas
        listOfNeighbours = hexagonGrid.GetNeighboursPositions(hexGridVectors);


        //hexgrid central points and its hexagon neigbours in one dictionary here
        Dictionary<int, List<Vector3>> r_dict = new();


        //init oozetypes here********************************************
        int r = UnityEngine.Random.Range(5, 6);

        OozeType[] oozeInstances = new OozeType[r];

        for (int i = 0; i < r; i++)
        {
            oozeInstances[i] = new OozeType(hexGridVectors, listOfNeighbours);
            r_dict.Add(i, oozeInstances[i].OozeProcess());

        }

        return r_dict;
    }

}
class Hexagon
{
    int m_gridX;
    int m_gridY;
    public List<Vector3> oozedHexagons;
    int mapSize;


    public Hexagon(int x, int y)
    {
        m_gridX = x; m_gridY = y;
        oozedHexagons = new List<Vector3>();
    }

    //construct grid
    public List<Vector3[]> ConstructGrid(int dimensionX)
    {

        //define general size of the single hexagon
        mapSize = GenerateMap.Mapwidth;
        int r = (int)(mapSize / m_gridX / 1.5f);
        m_gridX = (int)(mapSize / (r * 1.5));
        m_gridY = (int)(mapSize / (Mathf.Sqrt(3) * r));
        float m_hexagonHeight = Mathf.Sqrt(3) * r;
        float m_hexagonWidth = r * 2;

        if (m_gridX % 2 != 0) m_gridX += 1;
        if (m_gridY % 2 != 0) m_gridY -= 1;


        //define boundaries of the hexagons
        // center first then topleft clockwise all 6outer points +1center
        //allign with terrain + (terrainOffsets)


        Vector3[] offsets = {
            new Vector3(0+(m_hexagonWidth * .75f / 2),0,0+(m_hexagonHeight/2)),
            new Vector3(-r / 2+(m_hexagonWidth * .75f / 2), 50, +r / 2 * Mathf.Sqrt(3)+(m_hexagonHeight/2)) ,
            new Vector3(+r / 2+(m_hexagonWidth * .75f / 2), 50, +r / 2 * Mathf.Sqrt(3)+(m_hexagonHeight/2)),
            new Vector3(+r+(m_hexagonWidth * .75f / 2), 50, 0+(m_hexagonHeight/2)),
            new Vector3(+r / 2+(m_hexagonWidth * .75f / 2), 50, -r / 2 * Mathf.Sqrt(3)+(m_hexagonHeight/2)),
            new Vector3(-r / 2+(m_hexagonWidth * .75f / 2), 50, -r / 2 * Mathf.Sqrt(3)+(m_hexagonHeight/2)),
            new Vector3(-r+(m_hexagonWidth * .75f / 2), 50, 0+(m_hexagonHeight/2)) };

        //grid loop defining the grid inside a list: index1 is all hex points for topleft indexlast is bottom right hex points
        //top left to bot left* sorted

        List<Vector3[]> grid = new();

        for (int x = 0; x < m_gridX; ++x)
        {

            for (int y = 0; y < m_gridY; ++y)
            {
                //create array
                Vector3[] temp = new Vector3[7];
                System.Array.Fill(temp, new Vector3(m_hexagonWidth * .75f + (x * .75f * m_hexagonWidth), 0, m_hexagonHeight + (y * m_hexagonHeight)));

                //add offsets
                for (int i = 0; i < offsets.Length; ++i)
                {
                    temp[i] += offsets[i];
                }

                grid.Add(temp);

            }
        }
        return grid;
    }

    public List<Vector3[]> GetNeighboursPositions(List<Vector3[]> target)
    {

        int k = 0;
        List<Vector3[]> r_arr = new();
        for (int x = 0; x < m_gridX; ++x)
        {
            for (int y = 0; y < m_gridY; ++y)
            {
                //the 4 corner stones special cases
                //first
                if (x == 0 & y == 0)
                {
                    Vector3[] temp = { target.ElementAt(k + 1)[0], target.ElementAt(k + m_gridY)[0] };
                    r_arr.Add(temp);
                }
                //last
                else if (x == m_gridX - 1 && y == m_gridY - 1)
                {
                    Vector3[] temp = { target.ElementAt(k - 1)[0], target.ElementAt(k - m_gridY)[0] };
                    r_arr.Add(temp);
                }
                //bot left
                else if (x == 0 && y == m_gridY - 1)
                {
                    Vector3[] temp = { target.ElementAt(k - 1)[0], target.ElementAt(k + m_gridY - 1)[0], target.ElementAt(k + m_gridY)[0] };
                    r_arr.Add(temp);
                }
                //top right
                else if (x == m_gridX - 1 && y == 0)
                {
                    Vector3[] temp = { target.ElementAt(k + 1)[0], target.ElementAt(k - m_gridY + 1)[0], target.ElementAt(k - m_gridY)[0] };
                    r_arr.Add(temp);
                }
                //case 7: every stone not on the outer rows/columns
                else if (x != 0 && y != 0 && x != m_gridX - 1 && y != m_gridY - 1)
                {
                    Vector3[] temp =
                    {
                    target.ElementAt(k - m_gridY)[0],
                    target.ElementAt(k - m_gridY+1)[0],
                    target.ElementAt(k - 1)[0],
                    target.ElementAt(k + 1)[0],
                    target.ElementAt(k + m_gridY)[0],
                    target.ElementAt(k + m_gridY+ 1)[0],


                    };
                    r_arr.Add(temp);
                }
                //case6: last row every second -1
                else if (x % 2 == 0 && y == m_gridY - 1)
                {
                    Vector3[] temp =
                    {
                    target.ElementAt(k - m_gridY - 1)[0],
                    target.ElementAt(k - m_gridY)[0],
                    target.ElementAt(k - 1)[0],
                    target.ElementAt(k + m_gridY - 1)[0],
                    target.ElementAt(k + 1)[0]
                    };
                    r_arr.Add(temp);
                }
                //case 5: first row every second -1
                else if (x % 2 == 0 && y == 0)
                {

                    Vector3[] temp =
                    {
                    target.ElementAt(k - m_gridY)[0],
                    target.ElementAt(k + 1)[0],
                    target.ElementAt(k +m_gridY)[0],

                    };
                    r_arr.Add(temp);
                }
                //case 3: every second in last row
                else if (x % 2 != 0 && y == m_gridY - 1)
                {
                    Vector3[] temp =
                    {
                    target.ElementAt(k - m_gridY)[0],
                    target.ElementAt(k - 1)[0],
                    target.ElementAt(k + m_gridY)[0],

                    };
                    r_arr.Add(temp);
                }
                //case:2 every second in first row
                else if (x % 2 != 0 && y == 0)
                {
                    Vector3[] temp =
                    {
                    target.ElementAt(k - m_gridY)[0],
                    target.ElementAt(k - m_gridY+1)[0],
                    target.ElementAt(k + 1)[0],
                    target.ElementAt(k + m_gridY)[0],
                    target.ElementAt(k + m_gridY+ 1)[0],
                    };
                    r_arr.Add(temp);
                }
                //case 4: every stone in first column
                else if (x == 0)
                {
                    Vector3[] temp =
                    {
                    target.ElementAt(k - 1)[0],
                    target.ElementAt(k + 1)[0],
                    target.ElementAt(k + m_gridY)[0],
                    target.ElementAt(k + m_gridY- 1)[0],
                    };
                    r_arr.Add(temp);
                }
                //case ?: every stone i last column
                else if (x == m_gridX - 1)
                {
                    Vector3[] temp =
                    {
                    target.ElementAt(k - 1)[0],
                    target.ElementAt(k + 1)[0],
                    target.ElementAt(k - m_gridY)[0],
                    target.ElementAt(k - m_gridY + 1)[0],
                    };
                    r_arr.Add(temp);
                }

                ++k;
            }

        }

        return r_arr;
    }





    //public void SetAvailable(List<Vector3[]> x)
    //{
    //    for (int i = 0; i < x.Count; ++i)
    //    {
    //        availablePositions.Add(x[i].First());
    //    }
    //}

}

//erstes vertex array posi muss kleiner sein als verticesForBiome array size also: range[0,169sqrrt-rndmsize] angenommen 169vertexsizearray
// ausgew?hlte range dann auf y achse verschieben per multiplier(*169sqrrt) range[0,(169-rndmsizesqred/169sqrt)-1]