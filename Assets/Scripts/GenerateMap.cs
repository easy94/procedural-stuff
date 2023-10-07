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

    static Dictionary<Vector3, bool> checkList;

    public bool autoUpdate;


    public void DrawMapData()
    {
        noiseTexture = FindObjectOfType<DrawNoiseTexture>();

        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.ColorData = noiseTexture.DrawTexture(mapData.NoiseValueData);
        GenerateMesh.UpdateMesh(mapData.NoiseValueData, heightmultiplier, curve, LevelOfDetail, MapSizeMultiplier);
        MakeBiomes();
        PlaceAssets();


    }

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = GenerateNoise.Generate(Mapwidth*MapSizeMultiplier, Scale, octaves, freq, amp, seed);                                         
                                                                                              
        return noiseMap;
    }

    //chunk = tiles , tilesize is range between vertices aka scale of main Map
    //public void CreateChunkData()
    //{
    //    mapData.chunkVertexData = GenerateMesh.GenerateTile();
    //}

    public void MakeBiomes()
    {
        System.Random rand = new System.Random(seed);
        int amountOfBiomes = rand.Next(20, 30);
        int rndmIndex;
        List<Vector3> tempList = new();

        for (int i = 0; i < amountOfBiomes; i++)
        {
            rndmIndex = rand.Next(0, Biomes.Length);

            if (rndmIndex == 0)
            {
                tempList= GenerateBiomes.GenerateRndmBiomes(Biomes[0]);

                foreach (Vector3 item in tempList)
                {
                    Biomes[0].vertexPos.Add(item);
                }
                tempList.Clear();
            }
            else if (rndmIndex == 1)
            {
                tempList = GenerateBiomes.GenerateRndmBiomes(Biomes[1]);

                foreach (Vector3 item in tempList)
                {
                    Biomes[1].vertexPos.Add(item);
                }
                tempList.Clear();
            }
            else if (rndmIndex == 2)
            {
                tempList = GenerateBiomes.GenerateRndmBiomes(Biomes[2]);
                foreach (Vector3 item in tempList)
                {
                    Biomes[2].vertexPos.Add(item);
                }
                tempList.Clear();
            }
        }

    }
public void PlaceAssets()
{
            checkList = new Dictionary<Vector3, bool>();

        for (int i = 0; i < Biomes.Length; i++)
        {
            List<Vector3> arrayToIterate = Biomes[i].vertexPos;
            foreach (Vector3 e in arrayToIterate)
            {
                //improve this chance to generate asset later*****************************************************************************************************
                if(!checkList.ContainsKey(e))
                checkList.Add(e, true);
                Instantiate(Biomes[i].assets[Random.Range(0, Biomes[i].assets.Length)], new(e.x, e.y, e.z), Quaternion.Euler(0,Random.Range(0,181),0));
                
            }

        }

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
    public List<Vector3> vertexPos;

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