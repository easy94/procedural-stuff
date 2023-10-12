//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.VisualScripting;
//using UnityEngine;


//public static class Hexagon
//{
//    static int m_gridX;
//    static int m_gridY;

//    //construct grid
//    public static List<Vector3[]> ConstructGrid(float r, int dimensionX, int dimensionY)
//    {
//        //define general size of the single hexagon

//        m_gridX = dimensionX;
//        m_gridY = dimensionY;
//        float m_hexagonHeight = Mathf.Sqrt(3) * r;
//        float m_hexagonWidth = r * 2;
//        //float HexaCenterDistanceY = 

//        //define boundaries of the hexagons
//        // center first then topleft clockwise all 6outer points +1center

//        Vector3[] offsets = {
//            new Vector3(0,0,0),
//            new Vector3(-r / 2, 50, +r / 2 * Mathf.Sqrt(3)) ,
//            new Vector3(+r / 2, 50, +r / 2 * Mathf.Sqrt(3)),
//            new Vector3(+r, 50, 0),
//            new Vector3(+r / 2, 50, -r / 2 * Mathf.Sqrt(3)),
//            new Vector3(-r / 2, 50, -r / 2 * Mathf.Sqrt(3)),
//            new Vector3(-r, 50, 0) };

//        //grid loop defining the grid inside a list: index1 is all hex points for topleft indexlast is bottom right hex points
//        //top left to bot left* sorted

//        List<Vector3[]> grid = new();

//        int k = 0;
//        for (int x = 0; x < m_gridX; ++x)
//        {
//            for (int y = 0; y < m_gridY; ++y)
//            {
//                //create array
//                Vector3[] temp = new Vector3[7];
//                System.Array.Fill(temp, new Vector3(m_hexagonWidth * .75f * k, 0, m_hexagonHeight * k));

//                //add offsets
//                for (int i = 0; i < offsets.Length; i++)
//                {
//                    temp[i] += offsets[i];
//                }


//                grid.Add(temp);
//                ++k;
//            }
//        }

//        return grid;
//    }

//    public static List<Vector3[]> GetNeighboursPositions(List<Vector3[]> target)
//    {

//        int k = 0;
//        List<Vector3[]> r_arr = new();
//        for (int x = 0; x < m_gridX; ++x)
//        {
//            for (int y = 0; y < m_gridY; ++y)
//            {
//                //the 4 corner stones special cases
//                //first
//                if (x == 0 & y == 0)
//                {
//                    Vector3[] temp = { target.ElementAt(k + 1)[0], target.ElementAt(k + m_gridY)[0] };
//                    r_arr.Add(temp);
//                }
//                //last
//                else if (x == m_gridX - 1 && y == m_gridY - 1)
//                {
//                    Vector3[] temp = { target.ElementAt(k - 1)[0], target.ElementAt(k - m_gridY)[0] };
//                    r_arr.Add(temp);
//                }
//                //bot left
//                else if (x == 0 && y == m_gridY - 1)
//                {
//                    Vector3[] temp = { target.ElementAt(k - 1)[0], target.ElementAt(k + m_gridY - 1)[0], target.ElementAt(k + m_gridY)[0] };
//                    r_arr.Add(temp);
//                }
//                //top right
//                else if (x == m_gridX - 1 && y == 0)
//                {
//                    Vector3[] temp = { target.ElementAt(k + 1)[0], target.ElementAt(k - m_gridY + 1)[0], target.ElementAt(k - m_gridY)[0] };
//                    r_arr.Add(temp);
//                }
//                //case 7: every stone not on the outer rows/columns
//                else if (x != 0 && y != 0 && x != m_gridX - 1 && y != m_gridY - 1)
//                {
//                    Vector3[] temp =
//                    {
//                    target.ElementAt(k - m_gridY)[0],
//                    target.ElementAt(k - m_gridY+1)[0],
//                    target.ElementAt(k - 1)[0],
//                    target.ElementAt(k + 1)[0],
//                    target.ElementAt(k + m_gridY)[0],
//                    target.ElementAt(k + m_gridY+ 1)[0],


//                    };
//                    r_arr.Add(temp);
//                }
//                //case6: last row every second -1
//                else if (x % 2 == 0 && y == m_gridY - 1)
//                {
//                    Vector3[] temp =
//                    {
//                    target.ElementAt(k - m_gridY - 1)[0],
//                    target.ElementAt(k - m_gridY)[0],
//                    target.ElementAt(k - 1)[0],
//                    target.ElementAt(k + m_gridY - 1)[0],
//                    target.ElementAt(k + 1)[0]
//                    };
//                    r_arr.Add(temp);
//                }
//                //case 5: first row every second -1
//                else if (x % 2 == 0 && y == 0)
//                {

//                    Vector3[] temp =
//                    {
//                    target.ElementAt(k - m_gridY)[0],
//                    target.ElementAt(k + 1)[0],
//                    target.ElementAt(k +m_gridY)[0],

//                    };
//                    r_arr.Add(temp);
//                }
//                //case 3: every second in last row
//                else if (x % 2 != 0 && y == m_gridY - 1)
//                {
//                    Vector3[] temp =
//                    {
//                    target.ElementAt(k - m_gridY)[0],
//                    target.ElementAt(k - 1)[0],
//                    target.ElementAt(k + m_gridY)[0],

//                    };
//                    r_arr.Add(temp);
//                }
//                //case:2 every second in first row
//                else if (x % 2 != 0 && y == 0)
//                {
//                    Vector3[] temp =
//                    {
//                    target.ElementAt(k - m_gridY)[0],
//                    target.ElementAt(k - m_gridY+1)[0],
//                    target.ElementAt(k + 1)[0],
//                    target.ElementAt(k + m_gridY)[0],
//                    target.ElementAt(k + m_gridY+ 1)[0],
//                    };
//                    r_arr.Add(temp);
//                }
//                //case 4: every stone in first column
//                else if (x == 0)
//                {
//                    Vector3[] temp =
//                    {
//                    target.ElementAt(k - 1)[0],
//                    target.ElementAt(k + 1)[0],
//                    target.ElementAt(k + m_gridY)[0],
//                    target.ElementAt(k + m_gridY- 1)[0],
//                    };
//                    r_arr.Add(temp);
//                }
//                //case ?: every stone i last column
//                else if (x == m_gridX - 1)
//                {
//                    Vector3[] temp =
//                    {
//                    target.ElementAt(k - 1)[0],
//                    target.ElementAt(k + 1)[0],
//                    target.ElementAt(k - m_gridY)[0],
//                    target.ElementAt(k - m_gridY + 1)[0],
//                    };
//                    r_arr.Add(temp);
//                }

//                ++k;
//            }
            
//        }

//        return r_arr;
//    }

//    public static List<Vector3[]> OozeProcess()
//    {


//        return
//    }
//}
