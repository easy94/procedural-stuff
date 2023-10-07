using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public static class GenerateMesh
{

    public static void UpdateMesh(float[,] noiseMap, float heightmultiplier, AnimationCurve curve, int levelOfDetail, int multi)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        int reduceVertexFactor = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int vertexPerLine = ((noiseMap.GetLength(0) - 1) / reduceVertexFactor) + 1;

        Vector2[] uvArr = new Vector2[vertexPerLine * vertexPerLine];
        Vector3[] vertexArr = new Vector3[vertexPerLine * vertexPerLine];
        int[] triangleArr = new int[((vertexPerLine - 1) * (vertexPerLine - 1)) * 6];

        int j = 0;
        int k = 0;
        //generate vertices array + triangle array + uv array
        for (int x = 0; x < width; x += reduceVertexFactor)
        {
            for (int y = 0; y < height; y += reduceVertexFactor)
            {
                vertexArr[k] = new Vector3(x, (curve.Evaluate(noiseMap[x, y]) * heightmultiplier), y);
                uvArr[k] = new Vector2((float)x / width, (float)y / height);

                if (y < height - multi && x < height - multi)
                {
                    //first triangle

                    triangleArr[j] = k;
                    triangleArr[j + 1] = k + vertexPerLine + 1;
                    triangleArr[j + 2] = k + vertexPerLine;
                    //second triangle
                    triangleArr[j + 3] = k + vertexPerLine + 1;
                    triangleArr[j + 4] = k;
                    triangleArr[j + 5] = k + 1;

                    j += 6;
                }

                ++k;
            }
        }
        //myMesh.vertices = vertexArr;
        //myMesh.triangles = triangleArr;
        //myMesh.uv = uvArr;

        //NO NEED TO CREATE SHIT HERE CREATE IN GENERATE MAP INSTEAD OR GENERATE BIOMES?!
        GameObject terrainObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        terrainObject.GetComponent<MeshFilter>().sharedMesh.Clear();

        terrainObject.GetComponent<MeshFilter>().sharedMesh.vertices = vertexArr;
        terrainObject.GetComponent<MeshFilter>().sharedMesh.triangles = triangleArr;
        terrainObject.GetComponent<MeshFilter>().sharedMesh.uv = uvArr;

        terrainObject.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        terrainObject.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        //tempobj.transform.localScale = Vector3.one * multi;

        terrainObject.name = "test";

    }

    public static List<Vector3[]> GenerateTile()
    {

        //get vertices array from big terrain map in scene
        GameObject test = GameObject.Find("test");
        Vector3[] bigArr = test.GetComponent<MeshFilter>().sharedMesh.vertices;
        //small array is the array in order every 4 elements is one tile quad

        int sqrlngth = (int)Mathf.Sqrt(bigArr.Length);

        Vector3[] smallArr = new Vector3[(sqrlngth - 1) * (sqrlngth - 1) * 4];// 576length 169vertices
        int p = 0;
        int q = 0;

        for (int x = 1; x < sqrlngth - 1; ++x)
        {
            for (int y = 0; y < sqrlngth - 1; ++y)
            {
                smallArr[p] = bigArr[q];
                smallArr[p + 1] = bigArr[q + 1];
                smallArr[p + 2] = bigArr[sqrlngth + q];
                smallArr[p + 3] = bigArr[sqrlngth + 1 + q];
                p += 4;
                ++q;
            }
            ++q;
        }
        //small chunk array of vertice positions from big terrain to make many tile chunks
        List<Vector3[]> tileArrays = new List<Vector3[]>();
        for (int x = 0; x < (sqrlngth - 1) * (sqrlngth - 1); x += 4)
        {
            Vector3[] tempArr = new Vector3[4];
            tempArr[0] = smallArr[x];
            tempArr[1] = smallArr[x + 1];
            tempArr[2] = smallArr[x + 2];
            tempArr[3] = smallArr[x + 3];

            tileArrays.Add(tempArr);
        }

        return tileArrays;
    }

}





        //n.vertices = smallArr;
        //n.triangles = testtri;
        //n.uv = uv;


        //test1.GetComponent<MeshFilter>().sharedMesh = n;
        //test1.GetComponent<MeshCollider>().sharedMesh = n;