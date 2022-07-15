using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleFilter : MapGenSys.Filter
{
    
    override public string Name { get { return "Sample"; } }

    public float threshold = 0.0f;

    

    override public void Apply(ref MapGenSys.Data<float> data)
    {
        for (int i = 0; i < data.w * data.h; ++i)
        {
            if (data.data[i] < threshold)
            {
                data.data[i] = -1.0f;
            }
        }
    }
}