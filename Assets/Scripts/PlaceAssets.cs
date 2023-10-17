using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlaceAssets
{
    private static System.Random random;

    //make randompos before steepcheck
    public static Vector3[] CastRayOnTerrain(Vector3 x) //GetOozedPositions param
    {
        Vector3[] r_arr = new Vector3[1];

        random = new(Guid.NewGuid().GetHashCode());
        RaycastHit hit = new RaycastHit();

        Vector3 pos = RandomizePosition(x); // randomize position should return list of random vectors inside a hexagon
        Physics.Raycast(pos, Vector3.down, out hit, 150f);

        // isnotsteep should take list
        if (IsNotSteep(hit))
            x = hit.point;
        else
        {
            x = Vector3.zero;
        }

        //return array
        return r_arr;
    }

    private static bool IsNotSteep(RaycastHit x)
    {
        Vector3 normalDir = x.normal.normalized;

        if (Vector3.Dot(normalDir, Vector3.down.normalized) < -0.95f)
        {
            return true;
        }

        return false;
    }

    private static Vector3 RandomizePosition(Vector3 pos)
    {
        float x, y;
        x = random.Next(-15, 15);
        y = random.Next(-15, 15);

        return pos + new Vector3((float)random.NextDouble() * x, 0f, (float)random.NextDouble() * y);
    }

}