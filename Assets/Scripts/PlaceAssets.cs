using UnityEngine;

public static class PlaceAssets
{
    //make randompos before steepcheck
    public static Vector3 CastRayOnTerrain(Vector3 x)
    {

    //make the biomestencil tag at position 0 ignore raycast
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(x, Vector3.down, out hit, 150f, 0);

        if (IsNotSteep(hit))
        {
            x = hit.point;
            return x;
        }
        else
            x = default;

        return x;
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

}