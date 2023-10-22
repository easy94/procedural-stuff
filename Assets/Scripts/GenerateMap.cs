using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    private static System.Random random;

    public LevelData levelData;
    public BiomeData biomeData;
    [SerializeField] MeshCollider meshColliderTerrain;
    [SerializeField] MeshCollider meshColliderBiome;

    DrawNoiseTexture noiseTexture;

    [Range(1, 10)]
    [SerializeField] int MapSizeMultiplier;

    private int MapWidth = 961;

    MapData mapData = new MapData();

    GizmosDrawing drawing;

    public Biomes[] Biomes;

    [Range(6, 50)]
    [SerializeField] int HexGridX;

    public bool autoUpdate;

    OozeType Ooze;


    // test normals for steepness

    public void DrawMapData()
    {
        random = new(Guid.NewGuid().GetHashCode());
        // before generating destroy childobjects
        for (int i = meshColliderTerrain.transform.childCount; i > 0; --i)
            DestroyImmediate(meshColliderTerrain.transform.GetChild(0).gameObject);

        noiseTexture = FindObjectOfType<DrawNoiseTexture>();

        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.MeshData = GenerateMesh.UpdateMesh(mapData.NoiseValueData, levelData.LevelOfDetail, meshColliderTerrain, levelData.heightmultiplier, levelData.curve);

    }

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = GenerateNoise.Generate(MapWidth * MapSizeMultiplier, levelData.Scale, levelData.octaves, levelData.freq, levelData.amp, levelData.seed, levelData.offSetX, levelData.offSetY);

        return noiseMap;
    }

    public void MakeBiomes()
    {
        //int of dict-1 equals type of biom aka biom +1 == dict index
        Ooze = (GenerateBiomes.GenerateRndmBiomes(MapWidth, HexGridX, Biomes.Length));

        GenerateBiomeNoise();

        mapData.BiomeMeshData = GenerateMesh.UpdateMesh(mapData.BiomeNoise, 0, meshColliderBiome, 1, biomeData.curve);

        PlaceAsset();
    }

    public void GenerateBiomeNoise(float x = 0, float y = 0)
    {
        mapData.BiomeNoise = GenerateNoise.Generate(Mathf.RoundToInt(Ooze.GetRadius() * 2), biomeData.scale, biomeData.octaves, biomeData.frequency, biomeData.amplitude, random.Next(), biomeData.offsetX + x, biomeData.offsetY + y);
    }

    //improveable**************
    public void PlaceAsset()
    {

        //make one array for each biome type

        List<List<Vector3>> temp_list = new List<List<Vector3>>();

        List<Vector3> asset_positions = new List<Vector3>();

        int sizeX = mapData.BiomeNoise.GetLength(0);

        //temp_list is the positions the plane goes to. need for loop that loops through noise value arr and places at mesh vert position if val is >0.5
        temp_list = SortBiomeList(temp_list);


        //this algorith generates vertices to spawn objects on and sends them to the terrain position after checking noise val and steepness
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
                        if (mapData.BiomeNoise[x, y] > biomeData.spawnNoiseThreshhold && y % biomeData.skipVerts == 0)
                        {
                            //the positions that assets be placed on on the stencil
                            asset_positions.Add(mapData.BiomeMeshData[x * sizeX + y] + biomeStencil.transform.position);
                        }
                    }
                }
                Vector3 position = new();
                foreach (var v in asset_positions)
                {
                    //send the positions on the terrain before spawning
                    position = PlaceAssets.CastRayOnTerrain(v);
                    if (position != default)
                        Instantiate(Biomes[i].assets[random.Next(0, Biomes[i].assets.Length)], v, Quaternion.AngleAxis(random.Next(-45, 45), Vector3.up), meshColliderTerrain.transform);
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

        for (int i = 0; i < Ooze.iindex.Count; i++)
        {
            if (Ooze.iindex.ElementAt(i).Value == 1)
            {
                arg[0].Add(Ooze.iindex.Keys.ElementAt(i));

            }
            else if (Ooze.iindex.ElementAt(i).Value == 2)
            {
                arg[1].Add(Ooze.iindex.Keys.ElementAt(i));

            }
            else if (Ooze.iindex.ElementAt(i).Value == 3)
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
    public Biome Type;
}

public enum Biome
{
    Forest,
    Plain,

    Something,
}