using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class GenerateBiomes
{

    public static List<Vector3> GenerateRndmBiomes(Biomes biome)
    { 
        GameObject test = GameObject.Find("test");

        System.Random rand = new System.Random(Random.Range(-1000,1000));
        //find the vertex array from the terrain

        Vector3[] bigArr = test.GetComponent<MeshFilter>().sharedMesh.vertices;

        //array to hold biome vertices + variables for calculating max length and so on **** not finished because still need to figure how to find positions that are not yet taken by other biome
        int rndmSize = rand.Next(11,17);
        Vector3[] verticesForBiome = new Vector3[rndmSize*rndmSize];
        int firstVertexPos = Random.Range(0, (int)Mathf.Sqrt(bigArr.Length) - (int)Mathf.Sqrt(verticesForBiome.Length));
        int vertexPosMulti = Random.Range(0, (int)(bigArr.Length-((rndmSize*rndmSize)+(rndmSize*(int)Mathf.Sqrt(bigArr.Length)))) / ((int)Mathf.Sqrt(bigArr.Length)));
        int sqrlength = (int)Mathf.Sqrt(bigArr.Length);

        List<Vector3> verts = new List<Vector3>();

        //inside this loop i can manipulate the "biome" now *** whats missing is list of curves aka biomes that i can randomly use to manipulate verts and assets to spawn in specific biome
        for (int i = 0; i < rndmSize; ++i)
        {
            for (int j = 0; j < rndmSize; ++j)
            {
                //manipulate verts
                bigArr[(vertexPosMulti * sqrlength) + (i * sqrlength) + firstVertexPos + j].y *= 
                    biome.curve.Evaluate(bigArr[(vertexPosMulti * sqrlength) + (i * sqrlength) + firstVertexPos + j].y);
               
                //save verts position for instantiation in generatemap class
               verts.Add(bigArr[(vertexPosMulti * sqrlength) + (i * sqrlength) + firstVertexPos + j]);

            }
        }

        test.GetComponent<MeshFilter>().sharedMesh.vertices = bigArr;
        //test.GetComponent<MeshFilter>().sharedMesh.Clear();

        //test.GetComponent<MeshFilter>().sharedMesh.vertices = bigArr;

        test.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        test.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();

        return verts;
    }

    
    
}

        //erstes vertex array posi muss kleiner sein als verticesForBiome array size also: range[0,169sqrrt-rndmsize] angenommen 169vertexsizearray
        // ausgewählte range dann auf y achse verschieben per multiplier(*169sqrrt) range[0,(169-rndmsizesqred/169sqrt)-1]