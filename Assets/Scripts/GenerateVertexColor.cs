using System.Collections;
using UnityEngine;

public static class GenerateVertexColor
{
    public static Color[] PaintVerts(Vector3[] vertArr, Gradient grad)
    {
        
        Color[] vertexColors = new Color[vertArr.Length];
        float height;
        float min=float.MaxValue;
        float max=float.MinValue;
        for (int x = 0; x < vertArr.Length; ++x)
        {
            height = vertArr[x].y;

            if(height < min)
            {
                min = height;
            }

            if(height > max)
            {
                max = height;

            }

            vertexColors[x] = grad.Evaluate(Mathf.InverseLerp(min,max,height));
        }


        return vertexColors;
    }
    
}
