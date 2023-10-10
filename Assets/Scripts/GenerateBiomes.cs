using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class GenerateBiomes
{

    public static Vector3[] GenerateRndmBiomes(Biomes biome, Vector3[] terrainData, float hexaRadius)
    {
       Vector3[] hexaPositions = new Vector3[100];
        //flat top
        float heightDist = hexaRadius * Mathf.Sqrt(3);
        float widthDist = 1.5f * hexaRadius;
        int k = 0;
        for (int x = 0; x < 10; ++x)
        {
            for (int y = 0; y < 10; ++y)
            {
                hexaPositions[k] = new(widthDist*k, 0, heightDist*k);
                ++k;
            }
        }


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




        return hexaPositions;







        //GameObject test = GameObject.Find("test");

        //System.Random rand = new System.Random(Random.Range(-1000,1000));
        ////find the vertex array from the terrain

        //Vector3[] bigArr = test.GetComponent<MeshFilter>().sharedMesh.vertices;

        ////array to hold biome vertices + variables for calculating max length and so on **** not finished because still need to figure how to find positions that are not yet taken by other biome
        //int rndmSize = rand.Next(11,17);
        //Vector3[] verticesForBiome = new Vector3[rndmSize*rndmSize];
        //int firstVertexPos = Random.Range(0, (int)Mathf.Sqrt(bigArr.Length) - (int)Mathf.Sqrt(verticesForBiome.Length));
        //int vertexPosMulti = Random.Range(0, (int)(bigArr.Length-((rndmSize*rndmSize)+(rndmSize*(int)Mathf.Sqrt(bigArr.Length)))) / ((int)Mathf.Sqrt(bigArr.Length)));
        //int sqrlength = (int)Mathf.Sqrt(bigArr.Length);

        //List<Vector3> verts = new List<Vector3>();

        ////inside this loop i can manipulate the "biome" now *** whats missing is list of curves aka biomes that i can randomly use to manipulate verts and assets to spawn in specific biome
        //for (int i = 0; i < rndmSize; ++i)
        //{
        //    for (int j = 0; j < rndmSize; ++j)
        //    {
        //        //manipulate verts
        //        bigArr[(vertexPosMulti * sqrlength) + (i * sqrlength) + firstVertexPos + j].y *= 
        //            biome.curve.Evaluate(bigArr[(vertexPosMulti * sqrlength) + (i * sqrlength) + firstVertexPos + j].y);
               
        //        //save verts position for instantiation in generatemap class
        //       verts.Add(bigArr[(vertexPosMulti * sqrlength) + (i * sqrlength) + firstVertexPos + j]);

        //    }
        //}

        //test.GetComponent<MeshFilter>().sharedMesh.vertices = bigArr;
        ////test.GetComponent<MeshFilter>().sharedMesh.Clear();

        ////test.GetComponent<MeshFilter>().sharedMesh.vertices = bigArr;

        //test.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        //test.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();

        //return verts;
    }
}


        //erstes vertex array posi muss kleiner sein als verticesForBiome array size also: range[0,169sqrrt-rndmsize] angenommen 169vertexsizearray
        // ausgewählte range dann auf y achse verschieben per multiplier(*169sqrrt) range[0,(169-rndmsizesqred/169sqrt)-1]