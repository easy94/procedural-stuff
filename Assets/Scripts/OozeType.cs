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

    //for every hexagon its own "mystruct object"
    public List<Vector3> ooze_index = new List<Vector3>();
    public List<Vector3[]> oozeNeighbour_list = new List<Vector3[]>();
    public List<Vector3> oozedPositions = new List<Vector3>();
    public Vector3 sample;

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
       sample = ooze_index[UnityEngine.Random.Range(0, ooze_index.Count-1)];
    }


    public List<Vector3> OozeProcess()
    {
        UnityEngine.Random.InitState((short)(EditorApplication.timeSinceStartup * 7546987 / Mathf.Sqrt(10)));

        Vector3 sample = this.ooze_index[UnityEngine.Random.Range(0, this.ooze_index.Count - 1)];

       this.CalculateChance(9);

        return this.oozedPositions;
    }

    public List<Vector3> CalculateChance(short chance)
    { 

        UnityEngine.Random.InitState((short)(Time.timeAsDouble * Mathf.Sqrt(79) / Mathf.Sqrt(2)));

        for (int i = 0; i < this.ooze_index.Count; i++)
        {
            if (UnityEngine.Random.Range(0, chance++) < chance - i) //if success

                oozedPositions.Add(this.ooze_index[i]);
            return CalculateChance(GetTheNeighbours(this.ooze_index[i]), ++chance);
        }

        

        return null;
    }

    public List<Vector3> GetTheNeighbours(Vector3 target)
    {
        List<Vector3> result = new List<Vector3>();

        for (int i = 0; i < indexList.Count; i++)
        {
            Vector3 x = indexList[i];
            if (indexList[i] == (target))
            {
                foreach (Vector3 e in neighbourArr_list[i])
                {
                    result.Add(e);
                }
                break;
            }

        }
        //"result" is the list of neighbours
        //error here solution: loop through every neighbour of the target dont just return it. remove the
        //"target" in every neighbour. neighbour used as index in indexlist
        //error: removing neighbours wont work cause i have no hexagon objects with corresponding neigbours
        // solution: create hexagon class type to pass around as arrays instead of positions, and then 
        // delete the neighbours from that instance object

        for (int k = 0; k < result.Count; ++k)//find the corresponding neighbours
        {


        }

        for (int i = 0; i < result.Count; i++)
        {
            if (result[i] == (target))
            {
                result.RemoveAt(i);
                break;
            }
        }

        return result;
    }

    //add methods to remove neighbours and copy some shit from the struct***********************************

    //this method searches for the passed v3 in indexlist. since neighbourarr_list and indexlist elements allign with each other,
    // after i found the index i can just use the same index to get the neighbours. after that remove the hex i came from aka dont calc this target ever again
    // since its already oozed

    //public List<Vector3> GetTheNeighbours(Vector3 target)
    //{


    //}

}
