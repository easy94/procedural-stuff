using UnityEngine;


public class MyHexagon
{
    //auto properties
    public float Radius { get; set; }
    public float Height { get; set; }
    public float Width { get; set; }

    public int? Index { get; init; }

    public Vector3 Center { get; set; }
    public Vector3 TopRight { get; set; }
    public Vector3 TopLeft { get; set;}
    public Vector3 Right { get; set; }
    public Vector3 Left { get; set; }
    public Vector3 BotRight { get; set; }
    public Vector3 BotLeft { get;set; }

    //ctor
    public MyHexagon()
    {
    }

    public MyHexagon(float r, Vector3 pos, int? index = null)
    {
        Radius = r;
        Width = r * 2;
        Height = r * Mathf.Sqrt(3);
        Center = pos;
        TopLeft = pos + new Vector3(-r / 2, 100, +r / 2 * Mathf.Sqrt(3));
        TopRight = pos + new Vector3(+r / 2, 100, +r / 2 * Mathf.Sqrt(3));
        Right = pos + new Vector3(+r, 100, 0);
        BotRight = pos + new Vector3(+r / 2, 100, -r / 2 * Mathf.Sqrt(3));
        BotLeft = pos + new Vector3(-r / 2, 100, -r / 2 * Mathf.Sqrt(3));
        Left = pos + new Vector3(-r, 100, 0);
        Index = index;

    }

    //methods
    public bool IsInsideOfHexagon(Vector3 point)
    {
        //tophalf
        if ((point.z > Center.z) && point.z - Center.z < Mathf.Sqrt(3) * Radius / 2)
        {
            //somewhere in right half but not yet done
            if ((point.x > Center.x) && point.x - Center.x < Radius / 2)
            {
                if (Vector3.Dot((BotRight - TopRight).normalized, (point - TopRight).normalized) < .99f &&
                    Vector3.Dot((BotRight - TopRight).normalized, (point - TopRight).normalized) > .71f)
                {
                    return true;
                }
                else return false;
            }
            //left
            else if (point.x - Center.x > -Radius / 2)
            {
                if (Vector3.Dot((BotLeft - TopLeft).normalized, (point - TopLeft).normalized) < .99f &&
                   Vector3.Dot(BotLeft - TopLeft.normalized, (point - TopLeft).normalized) > .71f)
                {
                    return true;
                }
            }
            else return false;

        }
        //bothalf
        else if ((point.z < Center.z) && point.z - Center.z > -Mathf.Sqrt(3) * Radius / 2)
        {
            //right side
            if ((point.x > Center.x) && point.x - Center.x > Radius / 2)
            {
                if (Vector3.Dot((TopRight - BotRight).normalized, (point - BotRight).normalized) < .99f &&
                    Vector3.Dot((TopRight - BotRight).normalized, (point - BotRight).normalized) > .71f)
                {
                    return true;
                }
                else return false;
            }
            //leftside
            else if (point.x - Center.x < -Radius / 2)
            {
                if (Vector3.Dot((TopLeft - BotLeft).normalized, (point - BotLeft).normalized) < .99f &&
                    Vector3.Dot(TopLeft - BotLeft.normalized, (point - BotLeft).normalized) > .71f)
                {
                    return true;
                }
                else return false;

            }
            else return false;
        }

        return false;

    }

}

