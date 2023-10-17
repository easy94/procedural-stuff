using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlaceAssets
{
    private static readonly System.Random random = new(Guid.NewGuid().GetHashCode());

    //make randompos before steepcheck
    public static OozeType CastRayOnTerrain(OozeType ooze) //GetOozedPositions param
    {
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        _ = new RaycastHit();

        for (int i = 0; i < ooze.GetOozedPositions().Count; ++i)
        {
            Vector3 pos = RandomizePosition(ooze.GetOozedPositions()[i]);
            Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 55f);

            if (IsNotSteep(hit))
                ooze.GetOozedPositions()[i] = hit.point;
            else
            {
                ooze.GetOozedPositions().RemoveAt(i);
                Debug.Log(ooze.GetOozedPositions()[i]+"else");
            }
        }

        return ooze;
    }

    private static bool IsNotSteep(RaycastHit x)
    {
        Vector3 normalDir = x.normal.normalized;

        if (Vector3.Dot(normalDir, Vector3.down.normalized) < -0.8f)
        {
            return true;
        }

        return false;
    }

    private static Vector3 RandomizePosition(Vector3 pos)
    {
        float x, y;
        x = random.Next(0, 5);
        y = random.Next(0, 5);

        return pos + new Vector3((float)random.NextDouble() * x, 0f, (float)random.NextDouble() * y);
    }

}