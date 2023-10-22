using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class DrawNoiseTexture : MonoBehaviour
{

    public void DrawTexture(float[,] noiseMap, GameObject target)
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

        Texture2D texture = new(textureWidth, textureHeight);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();
        target.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;

    }
}
