using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OozeType:Hexagon
{
    //need one more list here: oozedneighbour list aka take oozedposition[i] -> search for the same in oindex -> get neighbours of that vector in neighbourlist
    //->add that vec to oozedneighbour if it is in oozedpositions also
    private List<Vector3> oindex;
    private List<List<Vector3>> oozeNeighbour_list;
    private List<Vector3> oozedPositions;
    

    //constructor

    public OozeType(List<Vector3[]> ind, List<Vector3[]> neighbours)
    {

        oindex = new List<Vector3>();
        oozeNeighbour_list = new List<List<Vector3>>();
        oozedPositions = new List<Vector3>();

        Vector3 point = new Vector3();
        point = ind.ElementAt(0)[0] - ind.ElementAt(0)[1];
        r = point.magnitude;

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

    public OozeType OozeProcess(int seed)
    {
        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());

        List<Vector3> list = new List<Vector3>();
        var shuffledList = this.oindex.OrderBy(_ => Guid.NewGuid()).ToList();

        Vector3 sample = shuffledList[rand.Next(0, this.oindex.Count)];
        NextRoundOfNeighbours(sample);
        
        this.CalculateChance(9, NextRoundOfNeighbours(sample));

        return this;
    }

    private void CalculateChance(int chance, List<Vector3> arg)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());

        if (arg.Count <= 0) return;
        for (int i = 0; i < arg.Count; ++i)
        {
            if (random.Next(0, chance) < 7 - i) //if success
            {
                oozedPositions.Add(arg[i]);//add the position to the return list
                CalculateChance(chance += 2, NextRoundOfNeighbours(arg[i]));
            }
        }
        return;
    }


    private List<Vector3> NextRoundOfNeighbours(Vector3 target)
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



}
