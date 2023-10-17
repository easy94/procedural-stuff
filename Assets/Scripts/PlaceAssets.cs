using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlaceAssets
{

    public static void CastRayOnTerrain(OozeType ooze) //GetOozedPositions param
    {
        System.Random rand = new System.Random();
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        RaycastHit hit = new RaycastHit();

        foreach (var item in ooze.GetOozedPositions())
        {
            Physics.Raycast(item,Vector3.down,out hit);
            if (IsNotSteep(hit))
            {

            }
        }

    }

    public static bool IsNotSteep(RaycastHit x)
    {


        return false;
    }
}

//CastRayFromAnRandomPosInsideOozedPosiOnTerrain()
//DetermineSteepness()
//PlaceAssetsInBoundsOfHexagonField()
//add rndmoffset to assets()
//place assets at min distance from each other script with ExecutesinEditMode "tag" ?!