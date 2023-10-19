using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlaceAssets
{

    //make randompos before steepcheck
    public static Vector3[] CastRayOnTerrain(Vector3 x, int density, float radius) //GetOozedPositions param
    {

        List<Vector3> r_arr = new();

        r_arr = RandomizePosition(x, density, radius); // randomize position should return list of random vectors inside a hexagon

        // isnotsteep should take list

        //return array
        return r_arr.ToArray();
    }

    private static bool IsNotSteep(RaycastHit x)
    {
        Vector3 normalDir = x.normal.normalized;
        float z = Vector3.Dot(normalDir, Vector3.down.normalized);
        if (Vector3.Dot(normalDir, Vector3.down.normalized) < -0.94f)
        {
            return true;
        }

        return false;
    }

    private static List<Vector3> RandomizePosition(Vector3 pos, int density, float r)
    {
        System.Random random = new(Guid.NewGuid().GetHashCode());

        //maximum acceptable range is equal to radius or height/2 in both directions
        float x, y;

        List<Vector3> temp = new();
        int cnt = 0;
        while (temp.Count <= density && cnt <= 10)
        {
        x = random.Next(Mathf.RoundToInt(-r), Mathf.RoundToInt(r));
        y = random.Next(Mathf.RoundToInt(-r * Mathf.Sqrt(3) / 2), Mathf.RoundToInt(r * Mathf.Sqrt(3) / 2));
            //find terrain steepness at posi
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(pos+new Vector3((float)random.NextDouble() * x, 0f, (float)random.NextDouble() * y), Vector3.down, out hit, 150f);

            if (IsNotSteep(hit))
            {
                temp.Add( hit.point);
            }
            ++cnt;
        }

        return temp;
    }

}