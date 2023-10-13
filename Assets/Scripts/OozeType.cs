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
    private List<Vector3> oindex = new();
    private List<List<Vector3>> oozeNeighbour_list = new();
    private List<Vector3> oozedPositions = new();

    //constructor

    public OozeType(List<Vector3[]> ind, List<Vector3[]> neigh)
    {

        for (int i = 0; i < ind.Count; ++i)
        {
            oindex.Add(ind[i].First());
        }

        for (int i = 0; i < oindex.Count; i++)
        {
            foreach (Vector3[] e in neigh)
            {

                oozeNeighbour_list.Add(e.ToList());

            }
        }
    }
    // compare oozed positions before adding to it?!
    // remove all neighbour refereces?! need to think
    //

    public List<Vector3> OozeProcess()
    {
        UnityEngine.Random.InitState((short)(EditorApplication.timeSinceStartup * 7546987 / Mathf.Sqrt(10)));

        Vector3 sample = this.oindex[UnityEngine.Random.Range(0, this.oindex.Count - 1)];
        GetTheNeighbours(sample);

        this.CalculateChance(9, GetTheNeighbours(sample));

        return this.oozedPositions;
    }

    private void CalculateChance(short chance, List<Vector3> arg)
    {

        UnityEngine.Random.InitState((short)(Time.timeAsDouble * Mathf.Sqrt(79) / Mathf.Sqrt(2)));


        for (int i = 0; i < arg.Count; i++)
        {
            if (UnityEngine.Random.Range(0, chance++) < chance - i) //if success
            {
                oozedPositions.Add(arg[i]);//add the position to the return list
                RemoveitFromPossibleOutcomes(arg[i]);// delete the position from possibly being inside a neighbour list of some friends
                CalculateChance(++chance, GetTheNeighbours(arg[i]));
            }
        }
        return;
    }


    private List<Vector3> GetTheNeighbours(Vector3 target)
    {
        List<Vector3> result = new();

        for (int i = 0; i < this.oindex.Count; i++)
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
            if(item.Contains(v))
            item.Remove(v);
        }
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
    //    return -1; // if this returns vector wasnt found
    //}


    //this method searches for the passed v3 in indexlist. since neighbourarr_list and indexlist elements allign with each other,
    // after i found the index i can just use the same index to get the neighbours. after that remove the hex i came from aka dont calc this target ever again
    // since its already oozed

}
