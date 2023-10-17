using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Hexagon
{
    protected float r;


    public List<Vector3[]> ConstructGrid(int size, int amountOfRows)
    {

        //define general size of the single hexagon

        r = (int)(size / amountOfRows / 1.5f);
        int m_gridX = (int)(size / (r * 1.5));
        int m_gridY = (int)(size / (Mathf.Sqrt(3) * r));
        float m_hexagonHeight = Mathf.Sqrt(3) * r;
        float m_hexagonWidth = r * 2;

        if (m_gridX % 2 != 0) m_gridX += 1;
        if (m_gridY % 2 != 0) m_gridY -= 1;


        //define boundaries of the hexagons
        // center first then topleft clockwise all 6outer points +1center
        //allign with terrain + (terrainOffsets)

        Vector3[] offsets = {
            new Vector3(0 - (m_hexagonWidth * .75f / 2),      100, 0 + (m_hexagonHeight / 2)),
            new Vector3(-r / 2 - (m_hexagonWidth * .75f / 2), 100, +r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(+r / 2 - (m_hexagonWidth * .75f / 2), 100, +r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(+r - (m_hexagonWidth * .75f / 2),     100, 0 + (m_hexagonHeight / 2)),
            new Vector3(+r / 2 - (m_hexagonWidth * .75f / 2), 100, -r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(-r / 2 - (m_hexagonWidth * .75f / 2), 100, -r / 2 * Mathf.Sqrt(3) + (m_hexagonHeight / 2)),
            new Vector3(-r - (m_hexagonWidth * .75f / 2),     100, 0 + (m_hexagonHeight / 2)) };



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

    public List<Vector3[]> GetNeighboursPositions(List<Vector3[]> target, int mapsize, int gridX)
    {
        int m_gridX = gridX;
        int m_gridY = (int)(mapsize / (Mathf.Sqrt(3) * r));

        if (m_gridX % 2 != 0) m_gridX += 1;
        if (m_gridY % 2 != 0) m_gridY -= 1;

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

    // maybe extend Bounds with dis code?
    public bool IsInsideOfHexagon(Vector3 point, Vector3 HexaCenterPos)
    {
        //tophalf
        if ((point.y > HexaCenterPos.y) && point.y - HexaCenterPos.y < Mathf.Sqrt(3) * r / 2)
        {
            //somewhere in right half but not yet done
            if ((point.x > HexaCenterPos.x) && point.x - HexaCenterPos.x > r / 2)
            {
                if (Vector3.Dot((GetBRF(HexaCenterPos) - GetTRF(HexaCenterPos)).normalized, (point - GetTRF(HexaCenterPos)).normalized) < .99f &&
                    Vector3.Dot((GetBRF(HexaCenterPos) - GetTRF(HexaCenterPos)).normalized, (point - GetTRF(HexaCenterPos)).normalized) > .71f)
                {
                    return true;
                }
                else return false;
            }
            //left
            else if (point.x - HexaCenterPos.x < -r / 2)
            {
                if (Vector3.Dot((GetBLF(HexaCenterPos) - GetTLF(HexaCenterPos)).normalized, (point - GetTLF(HexaCenterPos)).normalized) < .99f &&
                   Vector3.Dot((GetBLF(HexaCenterPos) - GetTLF(HexaCenterPos)).normalized, (point - GetTLF(HexaCenterPos)).normalized) > .71f)
                {
                    return true;
                }
            }
            else return false;


        }
        //bothalf
        else if ((point.y < HexaCenterPos.y) && point.y - HexaCenterPos.y > -Mathf.Sqrt(3) * r / 2)
        {
            //right side
            if ((point.x > HexaCenterPos.x) && point.x - HexaCenterPos.x > r / 2)
            {
                if (Vector3.Dot((GetTRF(HexaCenterPos) - GetBRF(HexaCenterPos)).normalized, (point - GetBRF(HexaCenterPos)).normalized) < .99f &&
                    Vector3.Dot((GetTRF(HexaCenterPos) - GetBRF(HexaCenterPos)).normalized, (point - GetBRF(HexaCenterPos)).normalized) > .71f)
                {
                    return true;
                }
                else return false;
            }
            //leftside
            else if (point.x - HexaCenterPos.x < -r / 2)
            {
                if (Vector3.Dot((GetTLF(HexaCenterPos) - GetBLF(HexaCenterPos)).normalized, (point - GetBLF(HexaCenterPos)).normalized) < .99f &&
                   Vector3.Dot((GetTLF(HexaCenterPos) - GetBLF(HexaCenterPos)).normalized, (point - GetBLF(HexaCenterPos)).normalized) > .71f)
                {
                    return true;
                }
                else return false;

            }
            else return false;
        }

        return false;
    }

    private Vector3 GetTLF(Vector3 arg)
    {
        return arg + new Vector3(-r / 2, 0, +r / 2 * Mathf.Sqrt(3));
    }
    private Vector3 GetTRF(Vector3 arg)
    {
        return arg + new Vector3(+r / 2, 0, +r / 2 * Mathf.Sqrt(3));
    }
    private Vector3 GetRF(Vector3 arg)
    {
        return arg + new Vector3(+r, 0, 0);
    }
    private Vector3 GetLF(Vector3 arg)
    {
        return arg + new Vector3(+r / 2, 0, -r / 2 * Mathf.Sqrt(3));
    }
    private Vector3 GetBRF(Vector3 arg)
    {
        return arg + new Vector3(-r / 2, 0, -r / 2 * Mathf.Sqrt(3));
    }
    private Vector3 GetBLF(Vector3 arg)
    {
        return arg + new Vector3(-r, 0f, 0f);
    }
}
