using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "LevelDataAsset")]
public class LevelData : ScriptableObject
{
    public AnimationCurve curve;
    public float Scale;
    public int offSetX;
    public int offSetY;
    [Range(0, 8)]
    public int LevelOfDetail;
    public int octaves;
    public float freq;
    public float amp;
    public int seed;
    public float heightmultiplier;
    public Gradient gradient;

}
