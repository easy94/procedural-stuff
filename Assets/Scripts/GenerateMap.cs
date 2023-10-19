using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    private static System.Random random;
    public LevelData levelData;

    [SerializeField] MeshCollider meshCollider;
    //DrawNoiseTexture noiseTexture;

    [Range(1, 10)]
    [SerializeField] int MapSizeMultiplier;
    public static int MapWidth = 961;

    MapData mapData = new MapData();

    GizmosDrawing drawing;

    public Biomes[] Biomes;

    [Range(1, 6)]
    public int assetDensity;

    [Range(6, 50)]
    [SerializeField] int HexGridX;

    public bool autoUpdate;

    OozeType Ooze;


    // test normals for steepness

    public void DrawMapData()
    {
        // before generating destroy childobjects
        for (int i = meshCollider.transform.childCount; i > 0; --i)
            DestroyImmediate(meshCollider.transform.GetChild(0).gameObject);

        //noiseTexture = FindObjectOfType<DrawNoiseTexture>();

        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.MeshData = GenerateMesh.UpdateMesh(mapData.NoiseValueData, levelData.heightmultiplier, levelData.curve, levelData.LevelOfDetail, meshCollider);
        mapData.ColorData = GenerateVertexColor.PaintVerts(mapData.MeshData, levelData.gradient);

        //meshCollider.GetComponent<MeshFilter>().sharedMesh.colors = mapData.ColorData;
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
        PlaceAsset();

    }


    //improveable**************
    public void PlaceAsset()
    {
        random = new(Guid.NewGuid().GetHashCode());

        //make one array for each biome type

        List<List<Vector3>> temp_list = new List<List<Vector3>>();

        temp_list = GeneratePositionsForAssets(temp_list);

        //finally place assets here
        for (int i = 0; i < Biomes.Length; i++)
        {
            foreach (var item in temp_list[i])
            {
                Instantiate(Biomes[i].assets[random.Next(0, Biomes[i].assets.Length)], item, Quaternion.AngleAxis(random.Next(-45, 45), Vector3.up), meshCollider.transform);
            }
        }

        drawing = GameObject.Find("Player").GetComponent<GizmosDrawing>();
        drawing.GetReference(Ooze.neighbours[11]);

    }

    private List<List<Vector3>> GeneratePositionsForAssets(List<List<Vector3>> arg)
    {

        for (int i = 0; i < Biomes.Length; i++)
            arg.Add(new List<Vector3>());

        for (int i = 0; i < Ooze.iindex.Count; i++)
        {
            if (Ooze.iindex.ElementAt(i).Value == 1)
            {
                arg[0] = arg[0].Concat(PlaceAssets.CastRayOnTerrain(Ooze.iindex.Keys.ElementAt(i), assetDensity, Ooze.rad)).ToList();
            }
            else if (Ooze.iindex.ElementAt(i).Value == 2)
            {
                arg[1] = arg[1].Concat(PlaceAssets.CastRayOnTerrain(Ooze.iindex.Keys.ElementAt(i), assetDensity, Ooze.rad)).ToList();
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
    public Color[] ColorData;
}

[System.Serializable]
public struct Biomes
{
    public string name;
    public GameObject[] assets;
}

enum Biome
{
    Forest,
    Plain,

    Something,
}