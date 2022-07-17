using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePatch : MonoBehaviour, MapGenSys.Algorithm
{
    // Configuration Values
    [SerializeField]
    private int neighborsRequired = 4;
    public int NeighborsRequired
    {
        get { return neighborsRequired; }
        set { neighborsRequired = value; dirty = true; }
    }
    public void SetNeighborsRequired(float value)
    {
        NeighborsRequired = (int)value;
    }

    [SerializeField]
    private int iterations = 1;
    public int Iterations
    {
        get { return iterations; }
        set { iterations = value; dirty = true; }
    }
    public void SetIterations(float value)
    {
        Iterations = (int)value;
    }


    // Alg Interface
    private bool dirty = true;

    [SerializeField]
    private bool _alg_enabled = true;
    public bool alg_enabled
    {
        get { return _alg_enabled; }
        set { _alg_enabled = value; dirty = true; }
    }
    public bool Dirty()
    {
        return dirty;
    }
    public string Name()
    {
        return "Propogate";
    }

    public void Apply(ref MapData<bool> data)
    {
        dirty = false;

        if (!alg_enabled) return;


        int counter = 0;
        while (counter < iterations)
        {
            data = DoIteration(data);
            ++counter;
        }
    }

    MapData<bool> DoIteration(MapData<bool> map)
    {
        MapData<bool> result = new(map.w, map.h);

        for (int i = 0; i < map.w * map.h; ++i)
        {
            // get index position
            MapPos gPos = new(i % map.w, i / map.w);

            bool posData = map.GetPos(gPos) ;

            result.SetPos(gPos, posData || result.GetPos(gPos));

            // only consider false tiles
            if (posData == true) continue;

            int neighbors_enabled = 0;

            // enable neighbors in temp
            for (int k = 0; k < 4; ++k)
            {
                MapPos eval = gPos + MapPos.Offsets[k];

                if (!result.ValidPos(eval)) continue;

                // count enabled neighbors
                if (map.GetPos(eval) == true)
                {
                    //result.SetPos(eval, true);
                    neighbors_enabled++;
                }
            }
        
            if (neighbors_enabled >= NeighborsRequired)
            {
                result.SetPos(gPos, true);
            }
        }

        return result;
    }
}
