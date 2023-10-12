using Random = System.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Rendering;
using System.Threading;

public static class GenerateBiomes
{
    public static Dictionary<Vector3, Vector3[]> GenerateRndmBiomes(Biomes biome, int gridX,int gridY, int seed)
    {
        Hexagon hexagonGrid = new Hexagon(gridX,gridY);

        //flat top hexagon grid positions. each hexagon has its own vector array with the first element being the center point
        List<Vector3[]> hexGridVectors;
        hexGridVectors = hexagonGrid.ConstructGrid(gridX);

        //name implies it all single vectors in one list
        hexagonGrid.SetAvailable(hexGridVectors);

        // get neighbours of each hexagon here example: first hexagon has 2 neigbours aka the central points of the adjacent hexas
        List<Vector3[]> ListOfNeighbours = hexagonGrid.GetNeighboursPositions(hexGridVectors);
        
        //hexgrid central points and its hexagon neigbours in one dictionary here
        Dictionary<Vector3, Vector3[]> r_dict = new Dictionary<Vector3, Vector3[]>();


        //init struct lists here
        for (int i = 0; i < hexGridVectors.Count; ++i)
        {
            hexagonGrid.hexagonStruct.indexList.Add(hexGridVectors[i].First());
        }
        for (int i = 0; i < hexagonGrid.hexagonStruct.indexList.Count; i++)
        {
            foreach (Vector3[] e in ListOfNeighbours)
            {
                foreach (Vector3 item in e)
                {
                    hexagonGrid.hexagonStruct.neighbourlist.Add(item);
                }
            }
        }

        //for (int i = 0; i < hexGridVectors.Count; ++i)
        //{
        //    r_dict.Add(hexGridVectors[i].First(), ListOfNeighbours[i]);
        //}


         //hexagonGrid.OozeProcess(seed);

        //danach alle positionen die geoozed worden sind nach dem rng move, speichern und in ein biom array packen damit items instantiatet werden können


        //Vector3 topLeftOffset = new Vector3(-hexaRadius, 50, +hexaRadius);
        //Vector3 topRightOffset = new Vector3(+hexaRadius, 50, +hexaRadius);
        //Vector3 left = new Vector3(-hexaRadius, 50, 0);
        //Vector3 right = new Vector3(+hexaRadius, 50, 0);
        //Vector3 botRightOffset = new Vector3(+hexaRadius, 50, -hexaRadius);
        //Vector3 botLeftOffset = new Vector3(-hexaRadius, 50, -hexaRadius);


        ////drawline for convinience

        //for (int i = 0; i < hexaPositions.Length; i++)
        //{
        //    Handles.DrawLine((hexaPositions[i] + topLeftOffset), (hexaPositions[i] + topRightOffset));
        //    Handles.DrawLine((hexaPositions[i] + topRightOffset), (hexaPositions[i] + right));
        //    Handles.DrawLine((hexaPositions[i] + right), (hexaPositions[i] + botRightOffset));
        //    Handles.DrawLine((hexaPositions[i] + botRightOffset), (hexaPositions[i] + botLeftOffset));
        //    Handles.DrawLine((hexaPositions[i] + botLeftOffset), (hexaPositions[i] + left));
        //    Handles.DrawLine((hexaPositions[i] + left), (hexaPositions[i] + topLeftOffset));
        //    Debug.Log("why");
        //}



        return r_dict;
    }
}
class Hexagon
{
    int m_gridX;
    int m_gridY;
    public List<Vector3> oozedHexagons;
    List<Vector3> availablePositions = new List<Vector3>();
    int mapSize;
   public MyStruct hexagonStruct;

    public Hexagon(int x,int y)
    {
        m_gridX = x; m_gridY = y;

        hexagonStruct = new MyStruct();
        hexagonStruct.indexList = new List<Vector3>();
        hexagonStruct.neighbourlist = new List<Vector3>();
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

        

        if (m_gridX % 2 != 0) m_gridX += 1;
        if (m_gridY % 2 != 0) m_gridY -= 1;


        for (int x = 0; x < m_gridX; ++x)
        {

            for (int y = 0; y < m_gridY; ++y)
            {
                //create array
                Vector3[] temp = new Vector3[7];
                System.Array.Fill(temp, new Vector3(m_hexagonWidth * .75f + (x*.75f*m_hexagonWidth), 0, m_hexagonHeight + (y*m_hexagonHeight)));

                //add offsets
                for (int i = 0; i < offsets.Length; ++i)
                {
                    temp[i] += offsets[i];
                }

                grid.Add(temp);

            }
        }

        foreach (Vector3[] e in grid)
        {
            foreach (Vector3 item in e)
            {
                Debug.Log(item);
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
    // this doestn work READ MORE RECURSIVE STUFF EVERYWHERE MAYBE ADD EVENT HERE SOMEWHERE?!??!?!!!
    public List<Vector3> OozeProcess(int seed)
    {
        System.Random rng = new System.Random(seed);
        var shuffled = availablePositions.OrderBy(_ => rng.Next()).ToList();
        availablePositions = shuffled;

        //cant think of a better solution than this
        List<Vector3> sample = new List<Vector3>();
           sample.Add(availablePositions[rng.Next(0, m_gridX*m_gridY)]);

        // how many bioms aka oozes
        for (int i = 0; i < rng.Next(10, 20); i++)
        {
            CalculateChance(sample);
        }

        return oozedHexagons;
    }

    // WHAT IS THE STOP CONDITION
    private List<Vector3> CalculateChance(List<Vector3> arg)
    {
        if(arg.Count ==0) return null;
        for (int i = 0; i < arg.Count; ++i)
        {
            int chance = 0;
            if (CoinFlip(chance)) //if success
            {
                ++chance;
                oozedHexagons.Add(arg[i]);
               return CalculateChance(hexagonStruct.GetTheNeighbours(arg,arg[i]));

            }
        }
        return null;
    }

    //private List<Vector3> GetNeigbours(Vector3 arg)
    //{

    //    //remove the Vector from the neighbours list because backtrack issue
    //    hexagonStruct.GetTheNeighbours()

    //    //return the list of neighbours
    //    neighbourList.TryGetValue(arg, out List<Vector3> r_List);

    //    return r_List;
    //}

    private bool CoinFlip(int rng)
    {
        UnityEngine.Random.Range(rng, 5);
        return UnityEngine.Random.Range(1, 10) < 10;
    }

    public void SetAvailable(List<Vector3[]> x)
    {
        foreach (var e in x)
            foreach (var item in e)
            {
                availablePositions.Add(item);
            }
    }

}
public struct MyStruct
{
        public List<Vector3> indexList;
        public List<Vector3> neighbourlist;


    public List<Vector3> GetTheNeighbours(List<Vector3> arg_list, Vector3 target)
    {
        List<Vector3> result = new List<Vector3>();

        for (int i = 0; i < indexList.Count; i++)
        {
            if (indexList[i].Equals(target))
            {
                result.Add(neighbourlist[i]);
            }
            
        }
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i].Equals(target))
            {
                result.RemoveAt(i);
            }
        }

        return result;
    }

}

//erstes vertex array posi muss kleiner sein als verticesForBiome array size also: range[0,169sqrrt-rndmsize] angenommen 169vertexsizearray
// ausgewählte range dann auf y achse verschieben per multiplier(*169sqrrt) range[0,(169-rndmsizesqred/169sqrt)-1]