using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OozeGrid:HexagonGrid
{
    private readonly List<MyHexagon> resetoindex;
    private readonly List<List<MyHexagon>> resetoozeNeighbour_list;
    
    private Dictionary<MyHexagon, BiomeEnum> _oozedFields = new Dictionary<MyHexagon, BiomeEnum>();
    public Dictionary<MyHexagon, BiomeEnum> OozedFields { get => _oozedFields; private set => _oozedFields = value; }

    //constructor
    public OozeGrid(int size, int x)
    {
        
        ConstructGrid(size, x, Vector3.zero);
        SetNeighboursPositions();

        //the new real index 
        foreach (var item in GridList)
        {
            OozedFields.Add(item, default);
        }

        resetoindex = GridList;
        resetoozeNeighbour_list = neighbours;

    }

    public OozeGrid OozeProcess(int biomIndex)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        MyHexagon sample = new();
        var shuffledList = this.GridList.OrderBy(_ => Guid.NewGuid()).ToList();
        sample = shuffledList[random.Next(0, this.GridList.Count)];

        
        this.CalculateChance(9, NextRoundOfNeighbours(sample), biomIndex);

        GridList = resetoindex;
        neighbours = resetoozeNeighbour_list;

        return this;
    }

    private void CalculateChance(int chance, List<MyHexagon> arg, int biomIndex)
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());

        if (arg.Count <= 0) return;
        for (int i = 0; i < arg.Count; ++i)
        {
            if (random.Next(0, chance) < 7 - i) //if success
            {
                OozedFields[arg[i]] = (BiomeEnum)biomIndex;//add the position to the return list
                CalculateChance(chance += 3, NextRoundOfNeighbours(arg[i]),biomIndex);
            }
        }
        return;
    }

    private List<MyHexagon> NextRoundOfNeighbours(MyHexagon target)
    {
        RemoveitFromPossibleOutcomes(target);// delete the position from possibly being inside a neighbour list of some friends

        List<MyHexagon> result = new();

        for (int i = 0; i < this.GridList.Count; ++i)
        {
            if (this.GridList[i] == (target))
            {
                result = this.neighbours[i];
                break;
            }
        }

        return result;
    }

    private void RemoveitFromPossibleOutcomes(MyHexagon v)
    {
        foreach (var item in neighbours)
        {
            if (item.Contains(v))
                item.Remove(v);
        }
    }

}
