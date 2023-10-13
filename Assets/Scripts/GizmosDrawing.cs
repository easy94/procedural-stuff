using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class GizmosDrawing : MonoBehaviour
{

    Dictionary<int, List<Vector3>> reference = new Dictionary<int, List<Vector3>>();



    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {

    float r = 1920/100*6;
    float heightDist = r * Mathf.Sqrt(3);
    float widthDist = 1.5f * r;
        Vector3[] hexaPositions = new Vector3[120];
        //flat top
        int k = 0;
        for (int x = 0; x < 12; ++x)
        {
            for (int y = 0; y < 10; ++y)
            {
                if(x%2==0 || x == 0)
                hexaPositions[k] = new((widthDist * x)+r/2/2, 0, (heightDist * y)-(heightDist/2)+(0.75f*heightDist));
                else
                    hexaPositions[k] = new(widthDist * x+r/2/2, 0, (heightDist * y)+0.75f*heightDist);
                ++k;
            }
        }

        Dictionary <Vector3,int> hexagonNeighbors = new Dictionary <Vector3,int>();


        Vector3 topLeftOffset = new Vector3(-r/2, 50, +r/2*Mathf.Sqrt(3));
        Vector3 topRightOffset = new Vector3(+r / 2, 50, +r/2*Mathf.Sqrt(3));
        Vector3 left = new Vector3(-r, 50, 0);
        Vector3 right = new Vector3(+r, 50, 0);
        Vector3 botRightOffset = new Vector3(+r / 2, 50, -r/2 * Mathf.Sqrt(3));
        Vector3 botLeftOffset = new Vector3(-r / 2, 50, -r/2 * Mathf.Sqrt(3));


        List<Vector3> list = new List<Vector3>();
        for (int i = 0;i<reference.ElementAt(0).Value.Count;i++)
        list.Add(reference.ElementAt(0).Value.ElementAt(i));

        StartCoroutine(Example(0, list));

    }

    IEnumerator Example(int index,List<Vector3> list)
    {
        while (index < list.Count)
        {
            yield return new EditorWaitForSeconds(2f);

                Gizmos.color = Color.green;
                Gizmos.DrawCube(list[index], new(100, 1, 100));
                ++index;
            Debug.Log("iwashere");

        }
    }


    public void GetReference(Dictionary<int, List<Vector3>> r_dict)
    {
        reference = r_dict;

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