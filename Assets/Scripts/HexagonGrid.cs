using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Grid
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    //how big are the grid fields
    public int Size { get; set; }


    public abstract List<Vector3[]> ConstructGrid(int amountOfCols, int amountOfRows, Vector3 startPos);
    public abstract List<Vector3[]> SetNeighboursPositions(int mapsize, int gridX);

}

public class HexagonGrid : Grid
{
    protected float r;
    public List<Vector3> centralPoints;
    protected List<Vector3[]> hexagonCorners;
    public List<List<Vector3>> neighbours;

    public List<Hexagon> GridList { get; set; }


    public override List<Vector3[]> ConstructGrid(int sizeM, int amountOfRows, Vector3 startPos)
    {

        neighbours = new();
        hexagonCorners = new();
        centralPoints = new();
        //define general size of the single hexagon

        Size = sizeM;
        r = (int)(Size / amountOfRows / 1.5f);
        GridX = (int)(Size / (r * 1.5));
        GridY = (int)(Size / (Mathf.Sqrt(3) * r));

        float h = Mathf.Sqrt(3) * r;
        float w = r * 2;

        if (GridX % 2 != 0) GridX += 1;
        if (GridY % 2 != 0) GridY += 1;
//////////////////////////////////////////////
        GridList = new();
        int j = 0;
        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                if (j % 2 == 0)
                GridList.Add(new Hexagon(r, startPos + new Vector3(x*r*.75f,0,y*), j));
            }
        }

        //define boundaries of the hexagons
        // center first then topleft clockwise all 6outer points +1center
        //allign with terrain + (terrainOffsets)

        Vector3[] offsets = {
            new Vector3(0 - (w * .75f / 2),      100, 0 + (h / 2)),
            new Vector3(-r / 2 - (w * .75f / 2), 100, +r / 2 * Mathf.Sqrt(3) + (h / 2)),
            new Vector3(+r / 2 - (w * .75f / 2), 100, +r / 2 * Mathf.Sqrt(3) + (h / 2)),
            new Vector3(+r - (w * .75f / 2),     100, 0 + (h / 2)),
            new Vector3(+r / 2 - (w * .75f / 2), 100, -r / 2 * Mathf.Sqrt(3) + (h / 2)),
            new Vector3(-r / 2 - (w * .75f / 2), 100, -r / 2 * Mathf.Sqrt(3) + (h / 2)),
            new Vector3(-r - (w * .75f / 2),     100, 0 + (h / 2)) };



        //grid loop defining the grid inside a list: index1 is all hex points for topleft indexlast is bottom right hex points
        //grid zählt in + Y = 1, also von unten links nach oben links, dann repeat +1X
        //top left to bot left* sorted

        List<Vector3[]> grid = new();

        for (int x = 0; x < GridX; ++x)
        {
            for (int y = 0; y < GridY; ++y)
            {
                //create array
                Vector3[] temp = new Vector3[7];
                System.Array.Fill(temp, new Vector3(w * .75f + (x * .75f * w), 0, h + (y * h)));

                //add offsets
                for (int i = 0; i < offsets.Length; ++i)
                {
                    temp[i] += offsets[i];
                }

                grid.Add(temp);

            }
        }

        hexagonCorners = grid;

        foreach (var item in grid)
        {
            centralPoints.Add(item.First());
        }
        return grid;
    }

    public override List<Vector3[]> SetNeighboursPositions(int mapsize, int gridX)
    {

        int k = 0;
        List<Vector3[]> r_arr = new();
        //all neighbour positions vector3's same order as the construct grid one
        for (int x = 0; x < gridX; ++x)
        {
            for (int y = 0; y < GridY; ++y)
            {
                //the 4 corner stones special cases
                //first
                if (x == 0 & y == 0)
                {
                    Vector3[] temp = { hexagonCorners.ElementAt(k + 1)[0], hexagonCorners.ElementAt(k + GridY)[0] };
                    r_arr.Add(temp);
                }
                //last
                else if (x == gridX - 1 && y == GridY - 1)
                {
                    Vector3[] temp = { hexagonCorners.ElementAt(k - 1)[0], hexagonCorners.ElementAt(k - GridY)[0] };
                    r_arr.Add(temp);
                }
                //bot left
                else if (x == 0 && y == GridY - 1)
                {
                    Vector3[] temp = { hexagonCorners.ElementAt(k - 1)[0], hexagonCorners.ElementAt(k + GridY - 1)[0], hexagonCorners.ElementAt(k + GridY)[0] };
                    r_arr.Add(temp);
                }
                //top right
                else if (x == gridX - 1 && y == 0)
                {
                    Vector3[] temp = { hexagonCorners.ElementAt(k + 1)[0], hexagonCorners.ElementAt(k - GridY + 1)[0], hexagonCorners.ElementAt(k - GridY)[0] };
                    r_arr.Add(temp);
                }
                //case 7: every stone not on the outer rows/columns
                else if (x != 0 && y != 0 && x != gridX - 1 && y != GridY - 1)
                {
                    Vector3[] temp =
                    {
                    hexagonCorners.ElementAt(k - GridY)[0],
                    hexagonCorners.ElementAt(k - GridY+1)[0],
                    hexagonCorners.ElementAt(k - 1)[0],
                    hexagonCorners.ElementAt(k + 1)[0],
                    hexagonCorners.ElementAt(k + GridY)[0],
                    hexagonCorners.ElementAt(k + GridY+ 1)[0],
                    };
                    r_arr.Add(temp);
                }
                //case6: last row every second -1
                else if (x % 2 == 0 && y == GridY - 1)
                {
                    Vector3[] temp =
                    {
                    hexagonCorners.ElementAt(k - GridY - 1)[0],
                    hexagonCorners.ElementAt(k - GridY)[0],
                    hexagonCorners.ElementAt(k - 1)[0],
                    hexagonCorners.ElementAt(k + GridY - 1)[0],
                    hexagonCorners.ElementAt(k + 1)[0]
                    };
                    r_arr.Add(temp);
                }
                //case 5: first row every second -1
                else if (x % 2 == 0 && y == 0)
                {

                    Vector3[] temp =
                    {
                    hexagonCorners.ElementAt(k - GridY)[0],
                    hexagonCorners.ElementAt(k + 1)[0],
                    hexagonCorners.ElementAt(k +GridY)[0],

                    };
                    r_arr.Add(temp);
                }
                //case 3: every second in last row
                else if (x % 2 != 0 && y == GridY - 1)
                {
                    Vector3[] temp =
                    {
                    hexagonCorners.ElementAt(k - GridY)[0],
                    hexagonCorners.ElementAt(k - 1)[0],
                    hexagonCorners.ElementAt(k + GridY)[0],

                    };
                    r_arr.Add(temp);
                }
                //case:2 every second in first row
                else if (x % 2 != 0 && y == 0)
                {
                    Vector3[] temp =
                    {
                    hexagonCorners.ElementAt(k - GridY)[0],
                    hexagonCorners.ElementAt(k - GridY+1)[0],
                    hexagonCorners.ElementAt(k + 1)[0],
                    hexagonCorners.ElementAt(k + GridY)[0],
                    hexagonCorners.ElementAt(k + GridY+ 1)[0],
                    };
                    r_arr.Add(temp);
                }
                //case 4: every stone in first column
                else if (x == 0)
                {
                    Vector3[] temp =
                    {
                    hexagonCorners.ElementAt(k - 1)[0],
                    hexagonCorners.ElementAt(k + 1)[0],
                    hexagonCorners.ElementAt(k + GridY)[0],
                    hexagonCorners.ElementAt(k + GridY- 1)[0],
                    };
                    r_arr.Add(temp);
                }
                //case ?: every stone i last column
                else if (x == gridX - 1)
                {
                    Vector3[] temp =
                    {
                    hexagonCorners.ElementAt(k - 1)[0],
                    hexagonCorners.ElementAt(k + 1)[0],
                    hexagonCorners.ElementAt(k - GridY)[0],
                    hexagonCorners.ElementAt(k - GridY + 1)[0],
                    };
                    r_arr.Add(temp);
                }

                ++k;
            }
        }

        foreach (Vector3[] item in r_arr)
        {
            neighbours.Add(item.ToList());
        }

        return r_arr;
    }

    public GameObject ConstructHexagonPlane(int r)
    {
        
        float m_hexagonHeight = Mathf.Sqrt(3) * r;
        float m_hexagonWidth = r * 2;
        
        Mesh myMesh = new Mesh();

        Vector3[] vertices = new Vector3[r * r];

        for (int x = 0; x < r; x++)
        {

        }

        Vector3[] verts = {
            new Vector3(-r / 2 , 100, +r / 2 * Mathf.Sqrt(3)), //TL
            new Vector3(+r / 2 , 100, +r / 2 * Mathf.Sqrt(3)), //TR
            new Vector3(+r ,     100, 0),                    //R
            new Vector3(+r / 2 , 100, -r / 2 * Mathf.Sqrt(3)), //BR
            new Vector3(-r / 2 , 100, -r / 2 * Mathf.Sqrt(3)), //BL
            new Vector3(-r,     100, 0),                      //L
            new Vector3(0 ,      100, 0) };                  //C

        int[] tris = new int[(verts.Length - 1) * 3];
        Vector2[] uvs = new Vector2[verts.Length];

        int i = 0;
        for (int x = 0; x < (verts.Length - 1); x++)
        {
            tris[i] = x;

            if (x % 5 == 0 && x!=0)
                tris[i + 1] = x - 5;
            else
                tris[i + 1] = x + 1;

            tris[i + 2] = 6;
            i += 3;
        }

        int k = 0;
        for (int j = 1; j <= verts.Length / 7; j++)
        {
            uvs[k].x = 0;
            uvs[k].y = 1;

            uvs[k + 1].x = .5f;
            uvs[k + 1].y = 1;

            uvs[k + 2].x = 1;
            uvs[k + 2].y = 0.5f;

            uvs[k + 3].x = 1;
            uvs[k + 3].y = 0;

            uvs[k + 4].x = .5f;
            uvs[k + 4].y = 0;

            uvs[k + 5].x = 0;
            uvs[k + 5].y = 0;

            uvs[k + 6].x = 0.5f;
            uvs[k + 6].y = 0.5f;

            k += 7;
        }

        GameObject objectt = GameObject.CreatePrimitive(PrimitiveType.Plane);
        myMesh.vertices = verts;
        myMesh.triangles = tris;
        myMesh.uv = uvs;
        myMesh.RecalculateBounds();
        myMesh.RecalculateNormals();
        objectt.GetComponent<MeshFilter>().mesh = myMesh;
        objectt.GetComponent<MeshCollider>().sharedMesh = myMesh;
        myMesh.name = "wtf";
        
        return objectt;
    }

    public float GetRadius()
    {
        return r;
    }
    public float GetHeight()
    {
        return r * Mathf.Sqrt(3);
    }
}
