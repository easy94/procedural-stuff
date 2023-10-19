using System.Collections.Generic;
using UnityEngine;

public class GizmosDrawing : MonoBehaviour
{

    List<Vector3> reference;



    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        //int j = 0;


        //Gizmos.color = Color.magenta;
        //for (int i = 0; i < reference.Count; i++)
        //{
        //    if (j == 0) Gizmos.color = Color.red;
        //    if (j == 1) Gizmos.color = Color.green;
        //    if (j == 2)
        //    {
        //        Gizmos.color = Color.blue;
        //        j = 0;
        //    }

        //    j++;

            foreach (Vector3 item in reference)
            {
                Gizmos.DrawCube(item + new Vector3(0,100,0), new(40, 1, 40));

            }


        //}

    }
    public void GetReference(List<Vector3> x)
    {
        reference = new List<Vector3>(); 
        reference = x;
    }
}

//Vector3 topLeftOffset = new Vector3(-hexaRadius, 50, +hexaRadius);
//Vector3 topRightOffset = new Vector3(+hexaRadius, 50, +hexaRadius);
//Vector3 left = new Vector3(-hexaRadius, 50, 0);
//Vector3 right = new Vector3(+hexaRadius, 50, 0);
//Vector3 botRightOffset = new Vector3(+hexaRadius, 50, -hexaRadius);
//Vector3 botLeftOffset = new Vector3(-hexaRadius, 50, -hexaRadius);


////drawline for convinience

//for (int i = 0; i < hexaPositions.Length; i++)
//{
//    Handles.DrawLine((hexaPositions[i] + topLeftOffset), (hexaPositions[i] + topRightOffset));
//    Handles.DrawLine((hexaPositions[i] + topRightOffset), (hexaPositions[i] + right));
//    Handles.DrawLine((hexaPositions[i] + right), (hexaPositions[i] + botRightOffset));
//    Handles.DrawLine((hexaPositions[i] + botRightOffset), (hexaPositions[i] + botLeftOffset));
//    Handles.DrawLine((hexaPositions[i] + botLeftOffset), (hexaPositions[i] + left));
//    Handles.DrawLine((hexaPositions[i] + left), (hexaPositions[i] + topLeftOffset));
//    Debug.Log("why");
//}