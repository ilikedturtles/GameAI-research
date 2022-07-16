using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAlgorithm : MonoBehaviour, MapGenSys.Algorithm
{
    //public string name = "Sample";

    private bool dirty = true;

    public string Name()
    {
        return "Sample";
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
    }
}
