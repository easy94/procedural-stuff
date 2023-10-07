using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GenerateBiomes
{

    public static void GenerateRndmBiomes()
    { 
        GameObject test = GameObject.Find("test");
       

        System.Random rand = new System.Random(Random.Range(-1000,1000));
        //find the vertex array from the terrain

        Vector3[] bigArr = test.GetComponent<MeshFilter>().sharedMesh.vertices;

        //array to hold biome vertices + variables for calculating max length and so on **** not finished because still need to figure how to find positions that are not yet taken by other biome
        int rndmSize = rand.Next(3, 7);
        Vector3[] verticesForBiome = new Vector3[rndmSize*rndmSize];
        int firstVertexPos = Random.Range(0, (int)Mathf.Sqrt(bigArr.Length) - (int)Mathf.Sqrt(verticesForBiome.Length));
        int vertexPosMulti = Random.Range(0, (int)(bigArr.Length-((rndmSize*rndmSize)+(rndmSize*(int)Mathf.Sqrt(bigArr.Length)))) / ((int)Mathf.Sqrt(bigArr.Length)));
        int sqrlength = (int)Mathf.Sqrt(bigArr.Length);
        int p = 0;

        Debug.Log(bigArr.Length+"bigarr length");
        Debug.Log(verticesForBiome.Length+ "new arr lenght");
        Debug.Log(rndmSize+"size for biome ");
        Debug.Log(firstVertexPos);
   
        Debug.Log(vertexPosMulti+"multiplier *" + sqrlength);

        //inside this loop i can manipulate the "biome" now *** whats missing is list of curves aka biomes that i can randomly use to manipulate verts and assets to spawn in specific biome
        for (int i = 0; i < rndmSize;++i)
        {
            for (int j = 0; j < rndmSize; ++j)
            {
                Debug.Log(p);
                Debug.Log((vertexPosMulti * sqrlength) + (i * sqrlength) + firstVertexPos + j);
                verticesForBiome[p] = bigArr[(vertexPosMulti*sqrlength)+(i*sqrlength)+firstVertexPos+j];
                ++p;

            }
        }
    }

}

        //erstes vertex array posi muss kleiner sein als verticesForBiome array size also: range[0,169sqrrt-rndmsize] angenommen 169vertexsizearray
        // ausgewählte range dann auf y achse verschieben per multiplier(*169sqrrt) range[0,(169-rndmsizesqred/169sqrt)-1]