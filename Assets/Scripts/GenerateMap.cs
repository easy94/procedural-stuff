using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// I want to add more asset arrays in biome struct so i can have more control over whats being spawned how many times and where
/// I want to add more asset arrays in biome struct so i can have more control over whats being spawned how many times and where
/// I want to add more asset arrays in biome struct so i can have more control over whats being spawned how many times and where

public class GenerateMap : MonoBehaviour
{
    private static System.Random random;

    public LevelData levelData;
    public BiomeData biomeData;
    [SerializeField] MeshCollider meshColliderTerrain;
    [SerializeField] MeshCollider meshColliderBiome;

    DrawNoiseTexture noiseTexture;
    GizmosDrawing drawing;

    [Range(1, 10)]
    [SerializeField] int MapSizeMultiplier;

    private readonly int MapWidth = 961;
    MapData mapData = new MapData();
    public Biomes[] Biomes;
    public bool autoUpdate;
    OozeGrid Ooze;



    //button1
    public void DrawMapData()
    {
        //initialize few vars
        noiseTexture = FindObjectOfType<DrawNoiseTexture>();
        random = new(Guid.NewGuid().GetHashCode());

        // before generating destroy childobjects
        for (int i = meshColliderTerrain.transform.childCount; i > 0; --i)
            DestroyImmediate(meshColliderTerrain.transform.GetChild(0).gameObject);


        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.MeshData = GenerateMesh.UpdateMesh(mapData.NoiseValueData, levelData.LevelOfDetail, meshColliderTerrain, levelData.heightmultiplier, levelData.curve);

    }
    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = GenerateNoise.Generate(MapWidth * MapSizeMultiplier, levelData.Scale, levelData.octaves, levelData.freq, levelData.amp, levelData.seed, levelData.offSetX, levelData.offSetY);

        return noiseMap;
    }


    //button2
    public void MakeBiomes()
    {
        //int of dict-1 equals type of biom aka biom +1 == dict index
        Ooze = GenerateBiomes.GenerateRndmBiomes(MapWidth, biomeData.HexGridX, Biomes.Length);

        GenerateBiomeNoise();

        mapData.BiomeMeshData = GenerateMesh.UpdateMesh(mapData.BiomeNoise, 0, meshColliderBiome, 1, biomeData.curve);

        PlaceAsset();
    }
    public void GenerateBiomeNoise(float x = 0, float y = 0)
    {
        mapData.BiomeNoise = GenerateNoise.Generate(Mathf.RoundToInt(Ooze.GetRadius() * 2), biomeData.scale, biomeData.octaves, biomeData.frequency, biomeData.amplitude, random.Next(), biomeData.offsetX + x, biomeData.offsetY + y);
    }

    public void PlaceAsset()
    {

        //temp_list is the positions the stencil goes to
        List<List<Vector3>> temp_list = new List<List<Vector3>>();
        temp_list = SortBiomeList(temp_list);


        //this algorithm generates vertices to spawn objects on and sends them to the terrain position after checking noise val and steepness
        List<Vector3> asset_positions = new List<Vector3>();
        int sizeX = mapData.BiomeNoise.GetLength(0);
        GameObject biomeStencil = GameObject.Find("BiomeStencil");
        Vector3 offset = new(Ooze.GetRadius(), 0, Ooze.GetHeight());

        for (int i = 0; i < temp_list.Count; i++)
        {
            foreach (Vector3 item in temp_list[i])
            {
                biomeStencil.transform.position = item - offset;

                GenerateBiomeNoise();

                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeX; y++)
                    {
                        if (mapData.BiomeNoise[x, y] > biomeData.spawnNoiseThreshhold && y % biomeData.spawnDensity == 0)
                        {
                            //the positions that assets be placed on on the stencil
                            asset_positions.Add(mapData.BiomeMeshData[x * sizeX + y] + biomeStencil.transform.position);
                        }
                    }
                }

                Vector3 position = new();
                foreach (var v in asset_positions)
                {
                    //send the positions on the terrain before spawning assets
                    position = PlaceAssets.CastRayOnTerrain(v);
                    if (position != default)
                        Instantiate(Biomes[i].assets[random.Next(0, Biomes[i].assets.Length)], position, Quaternion.AngleAxis(random.Next(-45, 45), Vector3.up), meshColliderTerrain.transform);
                }

                asset_positions.Clear();
            }

        }

    }

    private List<List<Vector3>> SortBiomeList(List<List<Vector3>> arg)
    {
        //necessary
        for (int i = 0; i < Biomes.Length; i++)
            arg.Add(new List<Vector3>());

        for (int i = 0; i < Ooze.oozedFields.Count; i++)
        {
            if (Ooze.oozedFields.ElementAt(i).Value == BiomeEnum.Forest)
            {
                arg[0].Add(Ooze.oozedFields.Keys.ElementAt(i));

            }
            else if (Ooze.oozedFields.ElementAt(i).Value == BiomeEnum.Plain)
            {
                arg[1].Add(Ooze.oozedFields.Keys.ElementAt(i));

            }
            else if (Ooze.oozedFields.ElementAt(i).Value == BiomeEnum.Something)
            {


            }
        }

        return arg;
    }

}

public struct MapData
{
    public float[,] NoiseValueData;
    public Vector3[] MeshData;

    public float[,] BiomeNoise;
    public Vector3[] BiomeMeshData;
}

[System.Serializable]
public struct Biomes
{
    public string name;
    public GameObject[] assets;
}