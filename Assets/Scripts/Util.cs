using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public static class Util
{

    //not working

    public static bool IsInsideOfHexagon(Vector3 point, Vector3 HexaCenterPos, float r)
    {
        //tophalf
        if ((point.z > HexaCenterPos.z) && point.z - HexaCenterPos.z < Mathf.Sqrt(3) * r / 2)
        {
            //somewhere in right half but not yet done
            if ((point.x > HexaCenterPos.x) && point.x - HexaCenterPos.x < r / 2)
            {
                if (Vector3.Dot((GetBRF() - GetTRF()).normalized, (point - GetTRF()).normalized) < .99f &&
                    Vector3.Dot((GetBRF() - GetTRF()).normalized, (point - GetTRF()).normalized) > .71f)
                {
                    return true;
                }
                else return false;
            }
            //left
            else if (point.x - HexaCenterPos.x > -r / 2)
            {
                if (Vector3.Dot((GetBLF() - GetTLF()).normalized, (point - GetTLF()).normalized) < .99f &&
                   Vector3.Dot(GetBLF() - GetTLF().normalized, (point - GetTLF()).normalized) > .71f)
                {
                    return true;
                }
            }
            else return false;

        }
        //bothalf
        else if ((point.z < HexaCenterPos.z) && point.z - HexaCenterPos.z > -Mathf.Sqrt(3) * r / 2)
        {
            //right side
            if ((point.x > HexaCenterPos.x) && point.x - HexaCenterPos.x > r / 2)
            {
                if (Vector3.Dot((GetTRF() - GetBRF()).normalized, (point - GetBRF()).normalized) < .99f &&
                    Vector3.Dot((GetTRF() - GetBRF()).normalized, (point - GetBRF()).normalized) > .71f)
                {
                    return true;
                }
                else return false;
            }
            //leftside
            else if (point.x - HexaCenterPos.x < -r / 2)
            {
                if (Vector3.Dot((GetTLF() - GetBLF()).normalized, (point - GetBLF()).normalized) < .99f &&
                    Vector3.Dot(GetTLF() - GetBLF().normalized, (point - GetBLF()).normalized) > .71f)
                {
                    return true;
                }
                else return false;

            }
            else return false;
        }

        return false;


        Vector3 GetTLF()
        {
            return HexaCenterPos + new Vector3(-r / 2, 0, +r / 2 * Mathf.Sqrt(3));
        }
        Vector3 GetTRF()
        {
            return HexaCenterPos + new Vector3(+r / 2, 0, +r / 2 * Mathf.Sqrt(3));
        }
        Vector3 GetBRF()
        {
            return HexaCenterPos + new Vector3(r / 2, 0, -r / 2 * Mathf.Sqrt(3));
        }
        Vector3 GetBLF()
        {
            return HexaCenterPos + new Vector3(-r / 2, 0f, -r / 2 * Mathf.Sqrt(3));
        }

    }


}