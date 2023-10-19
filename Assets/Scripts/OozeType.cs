using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OozeType:Hexagon
{
    //need one more list here: oozedneighbour list aka take oozedposition[i] -> search for the same in oindex -> get neighbours of that vector in neighbourlist
    //->add that vec to oozedneighbour if it is in oozedpositions also
    private readonly List<Vector3> resetoindex;
    public readonly List<List<Vector3>> resetoozeNeighbour_list;
    
    public Dictionary<Vector3, int> iindex = new Dictionary<Vector3, int>();

    //constructor
    public OozeType(int size, int x)
    {
        ConstructGrid(size, x);
        SetNeighboursPositions(size, x);

        //the new real index 
        foreach (Vector3 item in centralPoints)
        {
            iindex.Add(item, default);
        }

        resetoindex = centralPoints;
        resetoozeNeighbour_list = neighbours;

    }


    public OozeType OozeProcess(int biomIndex)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        Vector3 sample = new Vector3();
        var shuffledList = this.centralPoints.OrderBy(_ => Guid.NewGuid()).ToList();
        sample = shuffledList[random.Next(0, this.centralPoints.Count)];

        
        this.CalculateChance(9, NextRoundOfNeighbours(sample), biomIndex);

        centralPoints = resetoindex;
        neighbours = resetoozeNeighbour_list;

        return this;
    }

    private void CalculateChance(int chance, List<Vector3> arg, int biomIndex)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());

        if (arg.Count <= 0) return;
        for (int i = 0; i < arg.Count; ++i)
        {
            if (random.Next(0, chance) < 7 - i) //if success
            {
                iindex[arg[i]] = biomIndex;//add the position to the return list
                CalculateChance(chance += 3, NextRoundOfNeighbours(arg[i]),biomIndex);
            }
        }
        return;
    }

    private List<Vector3> NextRoundOfNeighbours(Vector3 target)
    {

        RemoveitFromPossibleOutcomes(target);// delete the position from possibly being inside a neighbour list of some friends

        List<Vector3> result = new();

        for (int i = 0; i < this.centralPoints.Count; ++i)
        {
            if (this.centralPoints[i] == (target))
            {
                result = this.neighbours[i];
                break;
            }
        }

        return result;
    }

    private void RemoveitFromPossibleOutcomes(Vector3 v)
    {
        foreach (var item in neighbours)
        {
            if (item.Contains(v))
                item.Remove(v);
        }
    }



}
