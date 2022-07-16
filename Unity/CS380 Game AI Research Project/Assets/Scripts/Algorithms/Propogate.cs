using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridPos = MapGenSys.GridPos;

public class Propogate : MonoBehaviour, MapGenSys.Algorithm
{
    private bool dirty = true;

    [SerializeField]
    private int iterations = 0;

    public int Iterations
    {
        get { return iterations; }
        set { iterations = value; dirty = true; }
    }
    
    public void SetIterations(float value)
    {
        Iterations = (int)value;
    }

    public string Name()
    {
        return "Propogate";
    }

    public bool Dirty()
    {
        return dirty;
    }

    public void Apply(ref MapGenSys.Data<bool> data)
    {
        dirty = false;

        for (int i = 0; i < data.w * data.h; ++i)
        {
            data.data[i] = !data.data[i];
        }

        //MapGenSys.Data<bool> newData = new(data.w, data.h);

        int counter = 0;
        while (counter < iterations)
        {
            data = DoIteration(data);
            //data = newData;
            counter++;
        }
    }

    MapGenSys.Data<bool> DoIteration(MapGenSys.Data<bool> map)
    {
        MapGenSys.Data<bool> result = new(map.w, map.h);

        for (int i = 0; i < map.w * map.h; ++i)
        {
            // get index position
            GridPos gPos = new(i % map.w, i / map.w);

            bool posData = map.GetPos(gPos) ;

            result.SetPos(gPos, posData || result.GetPos(gPos));

            if (posData == false) continue;

            // enable neighbors in temp
            for (int k = 0; k < 4; ++k)
            {
                GridPos eval = gPos + MapGenSys.offsets[k];

                if (!result.ValidPos(eval)) continue;

                // if neighbor is disabled, set enabled
                if (map.GetPos(eval) == false)
                {
                    result.SetPos(eval, true);
                }
            }
        }

        return result;
    }

}
