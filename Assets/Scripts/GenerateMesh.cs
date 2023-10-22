using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GenerateMesh
{

        public static float minMeshHeight;
        public static float maxMeshHeight;

    public static Vector3[] UpdateMesh(float[,] noiseMap, int levelOfDetail, MeshCollider terrainObject, float heightmultiplier, AnimationCurve curve = default)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        int reduceVertexFactor = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int vertexPerLine = ((noiseMap.GetLength(0) - 1) / reduceVertexFactor) + 1;


        Vector2[] uvArr = new Vector2[vertexPerLine * vertexPerLine];
        Vector3[] vertexArr = new Vector3[vertexPerLine * vertexPerLine];
        int[] triangleArr = new int[((vertexPerLine - 1) * (vertexPerLine - 1)) * 6];

        int j = 0;
        int k = 0;

        minMeshHeight = float.MaxValue;
        maxMeshHeight = float.MinValue;

        //generate vertices array + triangle array + uv array
        for (int x = 0; x < width; x += reduceVertexFactor)
        {
            for (int y = 0; y < height; y += reduceVertexFactor)
            {
                
                vertexArr[k] = new Vector3(x, (curve.Evaluate(noiseMap[x, y]) * heightmultiplier), y);
                uvArr[k] = new Vector2((float)x / width, (float)y / height);

                if (vertexArr[k].y > maxMeshHeight)
                    maxMeshHeight = vertexArr[k].y;
                if (vertexArr[k].y < minMeshHeight)
                    minMeshHeight = vertexArr[k].y;


                    if (y < height - 1 && x < height - 1)
                    {
                        //first triangle

                        triangleArr[j] = k;
                        triangleArr[j + 1] = k + vertexPerLine + 1;
                        triangleArr[j + 2] = k + vertexPerLine;
                        //second triangle
                        triangleArr[j + 3] = k + vertexPerLine + 1;
                        triangleArr[j + 4] = k;
                        triangleArr[j + 5] = k + 1;

                        j += 6;
                    }
                ++k;
            }
        }
        
        

        //remove
  

        terrainObject.GetComponent<MeshFilter>().sharedMesh.vertices = vertexArr;
        terrainObject.GetComponent<MeshFilter>().sharedMesh.triangles = triangleArr;
        terrainObject.GetComponent<MeshFilter>().sharedMesh.uv = uvArr;

        terrainObject.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        terrainObject.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();

        MeshCollider collider = terrainObject.GetComponent<MeshCollider>();
        collider.sharedMesh = terrainObject.GetComponent<MeshFilter>().sharedMesh;

        return vertexArr;
    }

   
}