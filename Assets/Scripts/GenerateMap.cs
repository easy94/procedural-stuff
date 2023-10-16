using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateMap : MonoBehaviour
{
    public LevelData levelData;

    [SerializeField] MeshCollider meshCollider;
    //DrawNoiseTexture noiseTexture;
    
    [Range(1, 10)]
    [SerializeField] int MapSizeMultiplier;
    public static int MapWidth = 961;

    MapData mapData = new MapData();
    public Biomes[] Biomes;

    [Range (6,50)]
    [SerializeField] int HexGridX;
    

    static Dictionary<Vector3, bool> checkList;

    public bool autoUpdate;

    // test normals for steepness

    public void DrawMapData()
    {
        //clear vertices before generating new mesh(from inspector) destroy childobjects
        for (int i = meshCollider.transform.childCount; i > 0; --i)
            DestroyImmediate(meshCollider.transform.GetChild(0).gameObject);

        //noiseTexture = FindObjectOfType<DrawNoiseTexture>();

        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.MeshData = GenerateMesh.UpdateMesh(mapData.NoiseValueData, levelData.heightmultiplier, levelData.curve, levelData.LevelOfDetail, meshCollider);
        // PlaceAssets();
        mapData.ColorData =GenerateVertexColor.PaintVerts(mapData.MeshData, levelData.gradient);
        meshCollider.GetComponent<MeshFilter>().sharedMesh.colors = mapData.ColorData;

    }

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = GenerateNoise.Generate(MapWidth * MapSizeMultiplier, levelData.Scale, levelData.octaves, levelData.freq, levelData.amp, levelData.seed, levelData.offSetX, levelData.offSetY);

        return noiseMap;
    }

    public void MakeBiomes()
    {
        //int of dict equals type of biom
        Dictionary<int, OozeType> BiomesDict = new();
        BiomesDict = GenerateBiomes.GenerateRndmBiomes(MapWidth,HexGridX, levelData.seed);
        


        //GameObject.Find("Player").GetComponent<GizmosDrawing>().GetReference(BiomesDict);
        //System.Random rand = new System.Random(seed);
        //int amountOfBiomes = rand.Next(15, 30);
        //int rndmIndex;
        //List<Vector3> tempList = new();

        //for (int i = 0; i < amountOfBiomes; i++)
        //{
        //    rndmIndex = rand.Next(0, Biomes.Length);
        //    if (rndmIndex == 0)
        //    {
        //        tempList = GenerateBiomes.GenerateRndmBiomes(Biomes[0]);
        //        foreach (Vector3 item in tempList)
        //        {
        //            Biomes[0].vertexPos.Add(item);
        //        }
        //        tempList.Clear();
        //    }
        //    else if (rndmIndex == 1)
        //    {
        //        tempList = GenerateBiomes.GenerateRndmBiomes(Biomes[1]);
        //        foreach (Vector3 item in tempList)
        //        {
        //            Biomes[1].vertexPos.Add(item);
        //        }
        //        tempList.Clear();
        //    }
        //    else if (rndmIndex == 2)
        //    {
        //        tempList = GenerateBiomes.GenerateRndmBiomes(Biomes[2]);
        //        foreach (Vector3 item in tempList)
        //        {
        //            Biomes[2].vertexPos.Add(item);
        //        }
        //        tempList.Clear();
        //    }
        //}

        ////get rid of stuttering in editor********************maybe reposition later **********
        //for (int i = 0; i < Biomes.Length; i++)
        //{
        //    Biomes[i].vertexPos.Clear();
        //}
    }
    
    //improveable**************
    public void PlaceAssets()
    {
        //to save positions where object already has been placed
        checkList = new Dictionary<Vector3, bool>();

        for (int i = 0; i < Biomes.Length; i++)
        {//get the vertexpositions this biom belongs to
            List<Vector3> arrayToIterate = Biomes[i].vertexPos;
            foreach (Vector3 e in arrayToIterate)
            {
                
                if (!checkList.ContainsKey(e))
                    checkList.Add(e, true);
                //50% chance to not place item after adding position to checklist
                if (Fiftyfifty()==true)
                {
                    GameObject newItem = Instantiate(Biomes[i].assets[Random.Range(0, Biomes[i].assets.Length)], new(e.x, e.y, e.z), Quaternion.Euler(0, Random.Range(0, 181), 0));
                    newItem.transform.SetParent(meshCollider.transform);
                }
            }
        }
        //for now this is best position for deleting stuttering editor
        for (int i = 0; i < Biomes.Length; i++)
        {
            Biomes[i].vertexPos.Clear();
        }

    }

    private bool Fiftyfifty()
    {
        return (Random.value * 1>.5f);
    }
}

public struct MapData
{
    public float[,] NoiseValueData;
    public Vector3[] MeshData;
    public Color[] ColorData;

    public MapData(float[,] noise, Color[] color, Vector3[] verts)
    {
        NoiseValueData = noise;
        ColorData = color;
        MeshData = verts;
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

//mapData.ColorData = noiseTexture.DrawTexture(mapData.NoiseValueData);



//MeshFilter tileInstanceMesh = tileInstance.GetComponent<MeshFilter>();
//tileInstanceMesh.mesh = GenerateMesh.UpdateMesh(data.NoiseValueData, heightmultiplier, myMeshFilter, curve, LevelOfDetail);

//MeshCollider tileInstanceCollider = tileInstance.GetComponent<MeshCollider>();
//tileInstanceCollider.sharedMesh = tileInstanceMesh.sharedMesh;


//myMeshFilter.mesh = GenerateMesh.UpdateMesh(data.NoiseValueData, heightmultiplier, myMeshFilter, curve, LevelOfDetail);
//MeshCollider meshCollider = myMeshFilter.GetComponent<MeshCollider>();
//meshCollider.sharedMesh = myMeshFilter.sharedMesh;