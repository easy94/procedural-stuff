using System.Collections;
using System.Collections.Generic;
using UnityEditor;                          //must be using UnityEditor
using UnityEngine;

[CustomEditor(typeof(GenerateMap))]         // this CustomEditor is type for the GenerateMap class functionality
public class MapGenEditor : Editor          //inherit from editor cause editorscripting
{
    public override void OnInspectorGUI()
    {
        GenerateMap mapgen = (GenerateMap)target;       //get reference to the class "GenerateMap"."(GenerateMap)cast" target is the object in scene being inspected ( perhaps not in scene necessary)

        if (DrawDefaultInspector())                     // draw default inspector and -
            if (mapgen.autoUpdate)                      // this works cause public bool in default inspector is ticket
            {
                mapgen.DrawMapData();
            }                                           // extra functionality for inspector:
        if (GUILayout.Button("Generate"))               //adding a button here -
        {
            mapgen.DrawMapData();
                                          //which calls function in GenerateMap class
        }
        //if (GUILayout.Button("GenerateHeight"))
        //{
        //    mapgen.GenerateHeightMap();
        //    if (mapgen == null)
        //        Debug.Log("generate noisemap first");
        //}
    }
}
