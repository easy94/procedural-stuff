using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Rendering;
using System.Threading;

public class OozeType : MonoBehaviour
{

    //for every hexagon its own "mystruct object"
    public List<Vector3> ooze_index = new List<Vector3>();
    public List<Vector3[]> oozeNeighbour_list = new List<Vector3[]>();
    public List<Vector3> oozedPositions = new List<Vector3>();

    //constructor

    public OozeType(List<Vector3[]> ind, List<Vector3[]> neigh)
    {
        
        for (int i = 0; i < ind.Count; ++i)
        {
            ooze_index.Add(ind[i].First());
        }

        for (int i = 0; i < ooze_index.Count; i++)
        {
            foreach (Vector3[] e in neigh)
            {

                oozeNeighbour_list.Add(e);

            }
        }
    }


    //add methods to remove neighbours and copy some shit from the struct***********************************

    //this method searches for the passed v3 in indexlist. since neighbourarr_list and indexlist elements allign with each other,
    // after i found the index i can just use the same index to get the neighbours. after that remove the hex i came from aka dont calc this target ever again
    // since its already oozed

    //public List<Vector3> GetTheNeighbours(Vector3 target)
    //{


    //}

}
