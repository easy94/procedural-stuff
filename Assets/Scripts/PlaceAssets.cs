using UnityEngine;

public static class PlaceAssets
{
    //make randompos before steepcheck
    public static Vector3 CastRayOnTerrain(Vector3 x)
    {
    //make the biomestencil tag at position 0 ignore raycast
        int layermask = ~(1 << 2);
        Physics.Raycast(x+ new Vector3(0,100,0), Vector3.down, out RaycastHit hit, 150f, layermask);

        if (IsNotSteep(hit))
        {
            x = hit.point;
            //event call here!!!!!!!!!
            return x;
        }
        else
            x = default;

        return x;
    }

    private static bool IsNotSteep(RaycastHit x)
    {
        Vector3 normalDir = x.normal.normalized;

        if (Vector3.Dot(normalDir, Vector3.down.normalized) < -0.94f)
        {
            return true;
        }

        return false;
    }

}