using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleFilter : MapGenSys.Filter
{
    //public string name = "Sample";

    override public string Name { get { return "Sample"; } }

    override public void Apply(ref MapGenSys.Data<float> data)
    {
        for (int i = 0; i < data.w * data.h; ++i)
        {
            if (data.data[i] < 0.5f)
            {
                data.data[i] = -1.0f;
            }
        }
    }
}