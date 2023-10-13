using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Rendering;
using System.Threading;
using UnityEditor;

public class OozeType : MonoBehaviour
{
    private List<Vector3> oindex;
    private List<List<Vector3>> oozeNeighbour_list;
    private List<Vector3> oozedPositions;

    //constructor

    public OozeType(List<Vector3[]> ind, List<Vector3[]> neighbours)
    {
        oindex = new List<Vector3>();
        oozedPositions = new List<Vector3>();
        oozeNeighbour_list = new List<List<Vector3>>();

        for (int i = 0; i < ind.Count; ++i)
        {
            oindex.Add(ind[i].First());
        }

        for (int i = 0; i < oindex.Count; ++i)
        {
            foreach (Vector3[] e in neighbours)
            {

                oozeNeighbour_list.Add(e.ToList());

            }
        }
    }

    public List<Vector3> OozeProcess()
    {
        UnityEngine.Random.InitState((int)(EditorApplication.timeSinceStartup * 7546987 / Mathf.Sqrt(10)));

        Vector3 sample = this.oindex[UnityEngine.Random.Range(0, this.oindex.Count - 1)];
        GetTheNeighbours(sample);

        this.CalculateChance(9, GetTheNeighbours(sample));

        return this.oozedPositions;
    }

    private void CalculateChance(int chance, List<Vector3> arg)
    {
        UnityEngine.Random.InitState((int)(EditorApplication.timeSinceStartup * 7546 / Mathf.Sqrt(2)));

        if (arg.Count <= 0) return;
        for (int i = 0; i < arg.Count; ++i)
        {
            if (UnityEngine.Random.Range(0, chance) < 7 - i) //if success
            {
                oozedPositions.Add(arg[i]);//add the position to the return list
                CalculateChance(chance+=2, GetTheNeighbours(arg[i]));
            }
        }
        return;
    }


    private List<Vector3> GetTheNeighbours(Vector3 target)
    {
        RemoveitFromPossibleOutcomes(target);// delete the position from possibly being inside a neighbour list of some friends
        List<Vector3> result = new();

        for (int i = 0; i < this.oindex.Count; ++i)
        {
            if (this.oindex[i] == (target))
            {
                result = this.oozeNeighbour_list[i];
                break;
            }
        }

        return result;
    }

    private void RemoveitFromPossibleOutcomes(Vector3 v)
    {
        foreach (var item in oozeNeighbour_list)
        {
            if (item.Contains(v))
                item.Remove(v);
        }
    }

    public void initializeOozeInstances()
    {

    }

    //private int GetIndexOfVectorPos(Vector3 x)
    //{
    //    for (int i = 0; i < this.oindex.Count; ++i)
    //    {
    //        if (this.oindex[i] == x)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1; // if this returns, vector wasnt found
    //}

}
