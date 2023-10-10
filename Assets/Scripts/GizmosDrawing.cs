using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GizmosDrawing : MonoBehaviour
{

    private void Start()
    {
        
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {

    float hexrad = 1920/100*6;
    float heightDist = hexrad * Mathf.Sqrt(3);
    float widthDist = 1.5f * hexrad;
        Vector3[] hexaPositions = new Vector3[120];
        //flat top
        int k = 0;
        for (int x = 0; x < 12; ++x)
        {
            for (int y = 0; y < 10; ++y)
            {
                if(x%2==0 || x == 0)
                hexaPositions[k] = new((widthDist * x)+hexrad/2/2, 0, (heightDist * y)-(heightDist/2)+(0.75f*heightDist));
                else
                    hexaPositions[k] = new(widthDist * x+hexrad/2/2, 0, (heightDist * y)+0.75f*heightDist);
                ++k;
            }
        }

        Dictionary <Vector3,int> hexagonNeighbors = new Dictionary <Vector3,int>();


        Vector3 topLeftOffset = new Vector3(-hexrad/2, 50, +hexrad/2*Mathf.Sqrt(3));
        Vector3 topRightOffset = new Vector3(+hexrad / 2, 50, +hexrad/2*Mathf.Sqrt(3));
        Vector3 left = new Vector3(-hexrad, 50, 0);
        Vector3 right = new Vector3(+hexrad, 50, 0);
        Vector3 botRightOffset = new Vector3(+hexrad / 2, 50, -hexrad/2 * Mathf.Sqrt(3));
        Vector3 botLeftOffset = new Vector3(-hexrad / 2, 50, -hexrad/2 * Mathf.Sqrt(3));

        Gizmos.color = Color.red;


        //drawline for convinience
        for(int i = 0;i < hexaPositions.Length;i++)
        {
            Gizmos.DrawLine((hexaPositions[i] + topLeftOffset), (hexaPositions[i] + topRightOffset));
            Gizmos.DrawLine((hexaPositions[i] + topRightOffset), (hexaPositions[i] + right));
            Gizmos.DrawLine((hexaPositions[i] + right), (hexaPositions[i] + botRightOffset));
            Gizmos.DrawLine((hexaPositions[i] + botRightOffset), (hexaPositions[i] + botLeftOffset));
            Gizmos.DrawLine((hexaPositions[i] + botLeftOffset), (hexaPositions[i] + left));
            Gizmos.DrawLine((hexaPositions[i] + left), (hexaPositions[i] + topLeftOffset)); 
        }

        


    }
}
