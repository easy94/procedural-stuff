using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class GenerateNoise       // static class cause only need one instance of this class
{

    public static float[,] Generate(int size, float scale, int octaves, float freq, float amp, int seed) // static needs to be cause class static
    {
        int sizeAfterMulti = size;

        Vector2[] offsetArr = new Vector2[octaves];
        System.Random random = new System.Random(seed);


        for(int i = 0; i < octaves; i++)
        {
            float rndmoffsetX = random.Next(-10000, 10000);
            float rndmoffsetY = random.Next(-10000, 10000);
            offsetArr[i] = new (rndmoffsetX, rndmoffsetY);
        }
        
        //create noisemap based on 145 base size multiplied by sizeMultiplier inserted in inspector
        float[,] noiseMap = new float[sizeAfterMulti, sizeAfterMulti];
        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        int halfwidth = sizeAfterMulti / 2;          // scale to center
        int halfheight = sizeAfterMulti / 2;

        for (int x = 0; x < sizeAfterMulti; x++)
        {
            for (int y = 0; y < sizeAfterMulti; y++)
            {
                float frequency = 1f;
                float amplitude = 1f;
                float noiseHeight = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    float tempX = (x-halfwidth) / scale * frequency+offsetArr[i].x;
                    float tempY = (y-halfheight) / scale * frequency+offsetArr[i].y;

                    float perlinVal = Mathf.PerlinNoise(tempY, tempX);

                    noiseHeight += perlinVal * amplitude;
                    amplitude *= amp;
                    frequency *= freq;
                }

                //remember lowest and highest values to inverse lerp through them (to stay in range of height aka less contrast)
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[y, x] = noiseHeight;
            }
        }
        for (int x = 0; x < sizeAfterMulti; x++)
        {
            for (int y = 0; y < sizeAfterMulti; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}
