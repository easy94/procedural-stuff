using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Hexagon
{
    int m_gridX;
    int m_gridY;
    int mapSize;
    float r;


    public Hexagon(int x, int y)
    {
        m_gridX = x; m_gridY = y;
    }

    //construct grid
    public List<Vector3[]> ConstructGrid(int gridx)
    {

        //define general size of the single hexagon
        mapSize = GenerateMap.Mapwidth;
        r = (int)(mapSize / m_gridX / 1.5f);
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
            new Vector3(0 - (m_hexagonWidth * .75f / 2), 0, 0 + (m_hexagonHeight / 2)),
            new Vector3(-r / 2 - (m_hexagonWidth * .75f / 2), 50, +r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(+r / 2 - (m_hexagonWidth * .75f / 2), 50, +r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(+r - (m_hexagonWidth * .75f / 2), 50, 0 + (m_hexagonHeight / 2)),
            new Vector3(+r / 2 - (m_hexagonWidth * .75f / 2), 50, -r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(-r / 2 - (m_hexagonWidth * .75f / 2), 50, -r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(-r - (m_hexagonWidth * .75f / 2), 50, 0 + (m_hexagonHeight / 2)) };

        //Vector3[] offsets = {
        //    new Vector3(0,0,0),
        //    new Vector3(-r / 2, 50, +r / 2 * Mathf.Sqrt(3)) ,
        //    new Vector3(+r / 2, 50, +r / 2 * Mathf.Sqrt(3)),
        //    new Vector3(+r, 50, 0),
        //    new Vector3(+r / 2, 50, -r / 2 * Mathf.Sqrt(3)),
        //    new Vector3(-r / 2, 50, -r / 2 * Mathf.Sqrt(3)),
        //    new Vector3(-r, 50, 0) };

        //grid loop defining the grid inside a list: index1 is all hex points for topleft indexlast is bottom right hex points
        //grid zählt in + Y = 1, also von unten links nach oben links, dann repeat +1X
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
        //all neighbour positions vector3's same order as the construct grid one
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

    public 

}
