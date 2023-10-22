using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeDataSO", menuName = "BiomeDataContainer")]
public class BiomeData : ScriptableObject
{
    public float scale;
    public float offsetX;
    public float offsetY;
    public int octaves;
    public float frequency;
    public float amplitude;
    public int seed;
    public AnimationCurve curve;
    [Range(1, 10f)]
    public int skipVerts;
    [Range(.5f,1f)]
    public float spawnNoiseThreshhold;
}