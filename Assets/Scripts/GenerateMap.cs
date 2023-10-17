using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

    public Biomes[] Biomes;

    [Range(6, 50)]
    [SerializeField] int HexGridX;

    public bool autoUpdate;

    OozeType Ooze;

    // test normals for steepness

    public void DrawMapData()
    {
        //clear vertices before generating new mesh(from inspector) destroy childobjects
        for (int i = meshCollider.transform.childCount; i > 0; --i)
            DestroyImmediate(meshCollider.transform.GetChild(0).gameObject);

        //noiseTexture = FindObjectOfType<DrawNoiseTexture>();

        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.MeshData = GenerateMesh.UpdateMesh(mapData.NoiseValueData, levelData.heightmultiplier, levelData.curve, levelData.LevelOfDetail, meshCollider);
        // PlaceAsset();
        mapData.ColorData = GenerateVertexColor.PaintVerts(mapData.MeshData, levelData.gradient);
        meshCollider.GetComponent<MeshFilter>().sharedMesh.colors = mapData.ColorData;

    }

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = GenerateNoise.Generate(MapWidth * MapSizeMultiplier, levelData.Scale, levelData.octaves, levelData.freq, levelData.amp, levelData.seed, levelData.offSetX, levelData.offSetY);

        return noiseMap;
    }

    public void MakeBiomes()
    {
        //int of dict-1 equals type of biom aka biom +1 == dict index
        
        Ooze = (GenerateBiomes.GenerateRndmBiomes(MapWidth, HexGridX, levelData.seed, Biomes.Length));
        PlaceAsset();
    }

    //improveable**************
    public void PlaceAsset()
    {
        random = new(Guid.NewGuid().GetHashCode());
        //make one array for each biome type
        List<Vector3[]> biomVertsByIndex = new List<Vector3[]>();

        for (int i = 0; i < Ooze.iindex.Count; i++)
        {       //need to determine which index belongs to which by dividing by amount of current biomes aka. every third is exmaple. forest every third+1 is village and so on
                biomVertsByIndex.Add(PlaceAssets.CastRayOnTerrain(Ooze.iindex.ElementAt(i).Key));
        }

        //rewrite this

        //for (int i = 0; i < OozeList.Count; i++)
        //{
        //    for (int j = 0; j < OozeList.ElementAt(i).GetOozedPositions().Count; j++)
        //    {
        //        if (j % 2 == 0 && OozeList.ElementAt(i).GetOozedPositions().ElementAt(j) != Vector3.zero)
        //            Instantiate(Biomes[0].assets[random.Next(0, 2)], OozeList.ElementAt(i).GetOozedPositions().ElementAt(j),
        //                Quaternion.AngleAxis(random.Next(-45,45), Vector3.up));

        //        if (j % 2 == 1 && OozeList.ElementAt(i).GetOozedPositions().ElementAt(j) != Vector3.zero)
        //            Instantiate(Biomes[1].assets[random.Next(0, 2)], OozeList.ElementAt(i).GetOozedPositions().ElementAt(j),
        //                Quaternion.AngleAxis(random.Next(-45, 45), Vector3.up));
        //    }
        //}

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
