using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateMap : MonoBehaviour
{
    public AnimationCurve curve;
    //[SerializeField] MeshFilter myMeshFilter;
    //[SerializeField] MeshCollider meshCollider;
    DrawNoiseTexture noiseTexture;
    [SerializeField] float Scale;
    [Range(1, 10)]
    [SerializeField] int MapSizeMultiplier;
    public static int Mapwidth = 145;
    [Range(0, 4)]
    [SerializeField] int LevelOfDetail;
    [SerializeField] int octaves;
    [SerializeField] float freq;
    [SerializeField] float amp;
    [SerializeField] int seed;
    [SerializeField] float heightmultiplier;
    MapData mapData= new MapData();

    public Biomes[] Biomes;

    //public GameObject[] biome1 = new GameObject[4];
    //public GameObject[] biome2 = new GameObject[4];
    //public GameObject[] biome3 = new GameObject[4];
    //public GameObject[] biome4 = new GameObject[4];

    public bool autoUpdate;


    public void DrawMapData()
    {
        noiseTexture = FindObjectOfType<DrawNoiseTexture>();

        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.ColorData = noiseTexture.DrawTexture(mapData.NoiseValueData);
        GenerateMesh.UpdateMesh(mapData.NoiseValueData, heightmultiplier, curve, LevelOfDetail, MapSizeMultiplier);
        MakeBiomes();


    }

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = GenerateNoise.Generate(Mapwidth*MapSizeMultiplier, Scale, octaves, freq, amp, seed);                                         
                                                                                              
        return noiseMap;
    }

    //chunk = tiles , tilesize is range between vertices aka scale of main Map
    public void CreateChunkData()
    {
        mapData.chunkVertexData = GenerateMesh.GenerateTile();
    }

    public void MakeBiomes()
    {
        
        int rndmIndex = Random.Range(0, 4);
        GenerateBiomes.GenerateRndmBiomes();
        
    }

}


public struct MapData
{
    public List<Vector3[]> chunkVertexData;
    public float[,] NoiseValueData;
    public Color32[] ColorData;


    public MapData(float[,] noise, Color32[] color, List<Vector3[]> chunks)
    {
        NoiseValueData = noise;
        ColorData = color;
        chunkVertexData = chunks;
    }
}
[System.Serializable]
public struct Biomes
{
    public string name;
    public AnimationCurve curve;
    public GameObject[] assets;
}


//MapData data = new MapData();
//data.NoiseValueData = GenerateNoiseMap();

//data.ColorData = noiseTexture.DrawTexture(data.NoiseValueData);



//MeshFilter tileInstanceMesh = tileInstance.GetComponent<MeshFilter>();
//tileInstanceMesh.mesh = GenerateMesh.UpdateMesh(data.NoiseValueData, heightmultiplier, myMeshFilter, curve, LevelOfDetail);

//MeshCollider tileInstanceCollider = tileInstance.GetComponent<MeshCollider>();
//tileInstanceCollider.sharedMesh = tileInstanceMesh.sharedMesh;


//myMeshFilter.mesh = GenerateMesh.UpdateMesh(data.NoiseValueData, heightmultiplier, myMeshFilter, curve, LevelOfDetail);
//MeshCollider meshCollider = myMeshFilter.GetComponent<MeshCollider>();
//meshCollider.sharedMesh = myMeshFilter.sharedMesh;