using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawNoiseTexture : MonoBehaviour
{

    public Color32[] DrawTexture(float[,] noiseMap)
    {
        int textureWidth = noiseMap.GetLength(0);
        int textureHeight = noiseMap.GetLength(1);

        Color32[] colorMap = new Color32[textureWidth * textureHeight];
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                colorMap[x * textureHeight + y] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }


       
        transform.localScale = new Vector3(textureHeight, 1, textureWidth);

        return colorMap;
    }


}
