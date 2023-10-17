using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OozeType:Hexagon
{
    //need one more list here: oozedneighbour list aka take oozedposition[i] -> search for the same in oindex -> get neighbours of that vector in neighbourlist
    //->add that vec to oozedneighbour if it is in oozedpositions also
    private readonly List<Vector3> resetoindex;
    private readonly List<List<Vector3>> resetoozeNeighbour_list;
    private List<Vector3> oindex;
    private List<List<Vector3>> oozeNeighbour_list;
    public Dictionary<Vector3, int> iindex;
    private static System.Random random;

    //constructor

    public OozeType(List<Vector3[]> ind, List<Vector3[]> neighbours)
    {
        random = new System.Random(Guid.NewGuid().GetHashCode());
        oindex = new List<Vector3>();
        oozeNeighbour_list = new List<List<Vector3>>();
        //oozedPositions = new List<Vector3>();

        //the new real index 
        foreach (Vector3[] item in ind)
        {
            iindex.Add(item.First(), default);
        }

        //radius
        Vector3 point = new Vector3();
        point = ind.ElementAt(0)[0] - ind.ElementAt(0)[1];
        r = point.magnitude;

        //oindex still used for checking 
        for (int i = 0; i < ind.Count; ++i)
        {
            oindex.Add(ind[i].First());
        }

        for (int i = 0; i < iindex.Count; ++i)
        {
            foreach (Vector3[] e in neighbours)
            {

                oozeNeighbour_list.Add(e.ToList());

            }
        }

        resetoindex = oindex;
        resetoozeNeighbour_list = oozeNeighbour_list;

    }

    public OozeType OozeProcess(int biomIndex)
    {
        var shuffledList = this.oindex.OrderBy(_ => Guid.NewGuid()).ToList();
        Vector3 sample = shuffledList[random.Next(0, this.oindex.Count)];


        NextRoundOfNeighbours(sample);
        
        this.CalculateChance(9, NextRoundOfNeighbours(sample), biomIndex);

        oindex = resetoindex;
        oozeNeighbour_list = resetoozeNeighbour_list;

        return this;
    }

    private void CalculateChance(int chance, List<Vector3> arg, int biomIndex)
    {

        if (arg.Count <= 0) return;
        for (int i = 0; i < arg.Count; ++i)
        {
            if (random.Next(0, chance) < 7 - i) //if success
            {
                iindex[arg[i]] = biomIndex;//add the position to the return list
                CalculateChance(chance += 2, NextRoundOfNeighbours(arg[i]),biomIndex);
            }
        }
        return;
    }

    //public void RemoveOoze(Vector3 target)
    //{
    //    this.oozedPositions.Remove(target);
    //}

    //public List<Vector3> GetOozedPositions()
    //{

    //    return this.oozedPositions;
    //}

    //public void SetOozePositions(List<Vector3> x)
    //{
    //    this.oozedPositions = x;
    //}

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
