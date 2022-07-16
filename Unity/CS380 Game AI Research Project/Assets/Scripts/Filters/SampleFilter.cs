using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleFilter : MonoBehaviour, MapGenSys.Filter
{
    private bool dirty = true;

    public string Name()
    {
        return "Sample";
    }

    public bool Dirty()
    {
        return dirty;
    }


    public float threshold = 0.0f;

    public void SetThreshold(float value)
    {
        dirty = true;
        threshold = value;
    }

    public void Apply(ref MapData<float> data)
    {
        dirty = false;

        for (int i = 0; i < data.w * data.h; ++i)
        {
            if (data.data[i] < threshold)
            {
                data.data[i] = -1.0f;
            }
        }
    }
}