using System.Collections.Generic;
using UnityEngine;

public class DrawNoiseTexture : MonoBehaviour
{

    public Color[] DrawTexture(float[,] noiseMap)
    {
        int textureWidth = noiseMap.GetLength(0);
        int textureHeight = noiseMap.GetLength(1);

        Color[] colorMap = new Color[textureWidth * textureHeight];
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


    public Texture2D DrawTexture(float[,] noiseMap, OozeType ooze)
    {
        int textureWidth = noiseMap.GetLength(0);
        int textureHeight = noiseMap.GetLength(1);

        Texture2D texture = new(textureWidth,textureHeight);

        Color[] colorMap = new Color[textureWidth*textureHeight];

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                colorMap[x * textureHeight + y] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }


        return texture;
    }

}
