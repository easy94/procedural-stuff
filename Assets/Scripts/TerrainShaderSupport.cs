using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// this script is used to supply the terrainshader's properties with values

public class TerrainShaderSupport : MonoBehaviour
{
    //DOESNT WORK YET CAUSE VALUES FORM EDITORMODE DONT GET SAVED INTO PLAYMODE
    Renderer myRenderer;
    Shader shader;
    public bool callOnvalidate;

    private void OnValidate()
    {
        myRenderer = GetComponent<Renderer>();
        shader = myRenderer.sharedMaterial.shader;

        int minYId = shader.GetPropertyNameId(0);
        int maxYId = shader.GetPropertyNameId(1);
        myRenderer.sharedMaterial.SetFloat(minYId, GenerateMesh.minMeshHeight);
        myRenderer.sharedMaterial.SetFloat(maxYId, GenerateMesh.maxMeshHeight);
    }
    
}
